## Instructions for how to clean up artwork

### Color adjustment

- When adjusting colors, be sure to check the "split view" to compare before / after
- Colors => "auto input levels" to get a starting point for restoration
	- Set "black point" by clicking on image (shadows, etc)
	- Set "white point" by clicking on image (collars, metal, etc)
	- Set "grey point" to get the mid-point of the image (most difficult to adjust, usually don't change it)
- Adjust saturation in Color => "Hue-Saturation"
	- First bump the saturation up for "master" to bring up everything
	- Then you can play with saturation of individual hues
	- Can make a custom selection using "lasso" tool, then adjust saturation of only the selected region
- Adjust Color => "Color Temperature" if needed to adjust on blue-yellow spectrum
- Adjust brightness / contrast of image or selected regions if needed


### Crop image

- At this point it is good to crop the image
- Image => "Canvas size" set to 822 x 1122 (the aspect ratio of the card)
	- "Link" the height and width
	- Set the height or width you want (the other will adjust)
	- Slide the canvas to where you want (can also use the move tool)
- Layer => "Layer to image size" to crop the image to the canvas size


### Repair image

- Healing tool
	- Select an area that is near identical, and not damaged as the source
	- Then click the area to repair to blend the source into it
- Selectively apply tools to the artwork
	- Create a copy of the artwork. The top layer is the "original" and the bottom layer is the one you apply tools to.
	- Apply the following tools if needed:
		- Filters => Despeckle
		- Layers => Artisitic => Oilfy
	- Then apply Eraser on the top layer to let the repair be shown through
		- For eraser, be sure to add alpha channel for jpegs
	- At the end of editing, right click top layer, then "merge down" to combine the images