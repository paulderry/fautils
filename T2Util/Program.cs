using System.Text;

// See https://aka.ms/new-console-template for more information
T2MapWriter map = new T2MapWriter("CUB.T2");
map.WriteArb(384,384,"384x384 Map", "CUB.PIC");

public class T2MapWriter {
    BinaryWriter writer;

    public T2MapWriter(String s) {
        writer = new BinaryWriter(File.Create(s));
    }

    /// <summary>
    /// This produces an unusable map file.
    /// </summary>
    public void Write256NoBorder() {
        writer.Write(Encoding.ASCII.GetBytes("BIT2"));
        writer.Write(Encoding.ASCII.GetBytes("Test Map".PadRight(80,(char)0x00)));
        writer.Write(Encoding.ASCII.GetBytes("CUB.PIC".PadRight(15, (char)0x00)));
        writer.Write((uint) 8192);
        writer.Write((uint) 0);
        writer.Write((uint) 8192);
        writer.Write(new byte[10]);
        writer.Write((uint) 8);
        writer.Write((uint) 16);
        writer.Write((uint) 32);
        writer.Write((uint) 196757);
        writer.Write((uint) 256);
        writer.Write((uint) 256);
        writer.Write((uint) 149);

        for (int x = 0; x < 256; x++) {
            for (int y = 0; y < 256; y++) {
                writer.Write((byte) 0xFF);
                writer.Write((byte) 0x01);
                writer.Write((byte) 0x00);
            }
        }
    }
        // 
        /// <summary>
        /// This produces an arbitrarily large map with the dimensions shown as x and y with a custom name and map picture image. 
        /// </summary>
        /// <param name="x">Tiles in the X axis.</param>
        /// <param name="y">Tiles in the Y axis</param>
        public void WriteArb(int x, int y, String mapName, string mapPic) {
        writer.Write(Encoding.ASCII.GetBytes("BIT2"));  // Magic
        writer.Write(Encoding.ASCII.GetBytes(mapName.PadRight(80, (char)0x00))); // In a good implementation we would check that mapName < 80 bytes
        writer.Write(Encoding.ASCII.GetBytes(mapPic.PadRight(15, (char)0x00))); // In a good implementation we would check that mapPic < 15 bytes
        writer.Write((uint) 8192); // Map picture dimension X
        writer.Write((uint) 0);    // No idea.
        writer.Write((uint) 8192); // Map picture dimension Y
        writer.Write(new byte[10]); // There's a big gap here.
        writer.Write((uint) 8); // Flag? I think they got lazy and made integer-width enums for something. Could end up looking like: 00111?
        writer.Write((uint) 16);    // Flag?
        writer.Write((uint) 32);    // Flag
        writer.Write((uint) (x*y*3) + 149); //442368 + 149 = 442517 assuming 384x384.
        writer.Write((uint) x); // X tiles
        writer.Write((uint) y); // Y tiles
        writer.Write((uint) 149); // Header length. If this value is not equal to the actual header length then you goofed badly.

        for (int xt = 0; xt < x; xt++) {
            for (int yt = 0; yt < y; yt++) {
                writer.Write((byte) Random.Shared.Next(0x00,0x20)); // Set the color randomly from 0-0x20
                writer.Write((byte) 0x01);  // Set the type to 0x01
                writer.Write((byte) Random.Shared.Next(0x00,0x10)); // Set the height randomly from 0-0x10 * 256
            }
        }
        // This fills in the rest. 
        //I'm not sure where these tiles are supposed to go. This will involve play testing.
        for (int edge = 0; edge < x*4; edge++) { 
                writer.Write((byte) 0xFF);
                writer.Write((byte) 0x01);
                writer.Write((byte) 0x00);     
        }
    }
}