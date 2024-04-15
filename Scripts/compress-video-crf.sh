#!/bin/bash

# Check if FFmpeg is installed
if ! command -v ffmpeg &> /dev/null
then
    echo "FFmpeg could not be found. Please install FFmpeg to use this script."
    exit 1
fi

# Check for at least one argument
if [ "$#" -lt 1 ]; then
    echo "Usage: $0 <input_file> [crf_value]"
    exit 1
fi

# Assign the input file to a variable
input_file="$1"

# Extract the file extension and base name
file_extension="${input_file##*.}"
file_base_name="${input_file%.*}"

# Generate the output file name
output_file="${file_base_name}.compressed.${file_extension}"

# Set the default CRF value
crf_value=23

# If a CRF value is provided, use it
if [ ! -z "$2" ]; then
    crf_value="$2"
fi

# Use FFmpeg to compress the video with the CRF flag
ffmpeg -i "$input_file" -vcodec libx264 -crf "$crf_value" -c:a copy "$output_file"

echo "Compression completed: ${output_file}"
