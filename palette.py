import struct
import sys
import codecs 

def load_palette(path):
	with open(path, mode='rb') as file:
		return file.read()

def read_triplets(binary_data):
	return struct.iter_unpack('BBB', binary_data)

def write_gimp_palette(triplets):
    with codecs.open(sys.argv[2], mode='w', encoding="utf-8") as file:
        file.write("GIMP Palette\n")
        file.write(f"Name: {sys.argv[1]}\n")
        for line in triplets:
            file.write(f"{line[0] << 2} {line[1] << 2} {line[2] << 2}\n")


def main():
    write_gimp_palette(read_triplets(load_palette(sys.argv[1])))

if __name__ == "__main__":
    main()
