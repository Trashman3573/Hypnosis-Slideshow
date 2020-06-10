# Hypnosis-Slideshow
Proof of concept windows client.

Click on the application in the taskbar and press Esc to exit fullscreen mode.

Use the left and right arrow keys to manually scroll through images.

The lower you set the delay between images the more the performance of your computer will suffer.

## Setting explanation:

### Speed:
Delay between images in milliseconds.
### Transparency:
% of transparency. 0 for for completely opaque and 100 for invisible. 
### Brainwash Mode:
Allows you to set a second directory from which an image will be shown at specified intervals.
### Deviation:
How often the brainwash images are shown. i.e "5" would make every 5th image a "brainwash" image. 
### Deviation Iinterval:
Time between brainwash increment intervals in minutes. i.e "15" would reduce the deviation to 4, meaning after 15 minutes every 4th image would be a "brainwash" image.
### Maximum Deviation:
A maximum amount the "Deviation Interval" can affect the "Deviation". i.e setting "Maximum Deviation" to "3" with "Deviation" set to "5" and a "Deviation Interval" of "15", would result in every 5th image being a "brainwash" image then after 15 minutes, then every 4th, then every 3rd, but would not increase any further. Setting this to "1" would eventually result in every image becoming a "brainwash" image. Set this to the same number as the "Deviation" setting to turn off this feature.

## I know this seems convoluted, im just terrible at explaining things. Latest release below:

https://github.com/Trashman3573/Hypnosis-Slideshow/releases
