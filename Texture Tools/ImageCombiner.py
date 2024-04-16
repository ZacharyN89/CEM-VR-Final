import os
import cv2
import re
import sys

def find_overlay_images(directory):
    """
    Finds and pairs F and R images within a directory.
    Returns a list of tuples: (F image path, R image path (None if not present), file name for output).
    """
    overlay_images_info = []
    images = os.listdir(directory)
    f_images = [img for img in images if img.endswith("-F.jpg")]

    for f_img_name in f_images:
        # Extract the file name according to the new specification
        match = re.search(r"P([A-E]|MA)-0-(\d{1,3})(-?[A-E]?)", f_img_name)
        if match:
            # Construct new file name, ensuring no extra dash is included
            extra_letter = match.group(3) if match.group(3) != "-" else ""
            file_name = f"{match.group(1)}-{match.group(2)}{extra_letter}-F"
        else:
            file_name = f_img_name[:-6]
            print(f"Could not parse filename {f_img_name}, using {file_name} as base name.")

        r_img_name = f_img_name.replace("-F.jpg", "-R.jpg")
        r_img_path = os.path.join(directory, r_img_name) if r_img_name in images else None
        f_img_path = os.path.join(directory, f_img_name)
        overlay_images_info.append((f_img_path, r_img_path, file_name))

        print(f"Matching: {f_img_name} {'with ' + r_img_name if r_img_path else ''}")

    return overlay_images_info

def combine_images(background_path, overlay_images_info, output_directory, positions_and_sizes, resolution):
    """
    Combines F and R images (if available) with a background image according to specified positions and sizes.
    Saves the output to png.
    """
    if not os.path.exists(output_directory):
        os.makedirs(output_directory)

    for f_path, r_path, file_name in overlay_images_info:
        # Reload background for each combination to avoid overlaying on top of previous images
        background = cv2.imread(background_path)

        for overlay_path, (x, y, w, h) in zip([f_path, r_path] if r_path else [f_path], positions_and_sizes):
            if overlay_path:
                overlay = cv2.imread(overlay_path)
                overlay = cv2.resize(overlay, (w, h))

                if x + w > background.shape[1] or y + h > background.shape[0]:
                    print(f"Overlay image {os.path.basename(overlay_path)} exceeds background dimensions. Skipping.")
                    continue

                # Perform the overlay
                background[y:y+h, x:x+w] = overlay
                print(f"Combining: {os.path.basename(overlay_path)} onto background")

        # Resize Texture
        background = cv2.resize(background, (resolution, resolution))
        print(f"Resizing: {file_name} to be {resolution}x{resolution}")

        # Save the combined image with the new naming convention, ensuring no extra dashes
        output_path = os.path.join(output_directory, f"{file_name}.png")
        cv2.imwrite(output_path, background)
        print(f"Saved combined image as {output_path}")

# Usage
directory_with_images = sys.argv[1]
output_directory = 'FinalTextures'
background_image_path = 'plainHeadstone.png'
overlay_images_info = find_overlay_images(directory_with_images)
positions_and_sizes = [(0, 0, 1005, 1590), (1005, 0, 1043, 1590)]  # First for F, second for R if present
resolution = 512
combine_images(background_image_path, overlay_images_info, output_directory, positions_and_sizes, resolution)
