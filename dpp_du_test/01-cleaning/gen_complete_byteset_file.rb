# seed the random number generator
srand(95426758259632547)

# create an array of all bytes
bytes=*(0..255)
bytes.shuffle!

# create and populate the result file
File.open("complete_byte_set_file", 'w') { |file|
    # the result file must contain every byte at least once
    while(!bytes.empty?)
        # either get rid another of the bytes or write a random byte, based on chance
        file.write((rand(2) ? bytes.shift : rand(256)).chr)
    end
}
