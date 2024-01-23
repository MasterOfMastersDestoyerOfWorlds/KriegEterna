  (script-fu-register
    "script-fu-outline-layer"                    ;function name
    "Outline Layer"                                  ;menu label
    "Creates Title Text"              ;description
    "Andrew Wollack"                             ;author
    "Andrew Wollack"        ;copyright notice
    "October 27, 1997"                          ;date created
    "*"                                      ;image type that the script works on
	SF-IMAGE       "Input image" 0
	SF-GRADIENT    "Gradient" 	   "Weather-Title"					;title-gradient		need
	SF-STRING     "OutlineDark"       "TRUE" 	;outline dark (true) or Light(false flag
  	SF-ADJUSTMENT _"Border size (pixels)" '(12 1 250 1 10 0 1)

  )
  (script-fu-menu-register "script-fu-outline-layer" "<Image>/Layer")

  
  (define (script-fu-outline-layer image title-gradient outline-dark border-size)
    (let*
      (
        ; define our local variables
        ; create a new image:
        (image-width  10)
        (image-height 10)
		(crop-width  10)
        (crop-height 10)
		(title-width  10)
        (title-height 10)
		(strength-width 10)
		(strength-height 10)
		(back-opacity 80); was 70
		(black-border-text FALSE)
		(inBufferAmount 108)
		(yfact 20)
		(boxFac)
        (buffer)           ;create a new layer for the image
		(title-size)
        (layer
			    (car
					(gimp-image-get-active-layer image)
                )
		)
		(title-back-layer)
		(title-first-outline-layer)
		(title-second-outline-layer)
		(title-gradient-layer)
		(merge-layer)
      ) ;end of our local variables
	  
	  
		(gimp-image-undo-group-start image)
		(gimp-selection-none image)
	  (set! image-width   (car (gimp-image-width  image) ) )
	  (set! image-height  (car (gimp-image-height image) ) )
	  

	(set! buffer inBufferAmount)
	  
	  ;;Title  formatting
	  (set! title-size image-width) 
	  ;Make vanilla title text
		
		(set! title-width   (car (gimp-drawable-width  layer) ) )
		(set! title-height  (car (gimp-drawable-height layer) ) )
		(set! title-height (+ title-height buffer buffer) )
		(set! title-width  (+ title-width  buffer buffer) )
		(gimp-layer-set-offsets layer (/ title-height 8) (/ title-width 8))
		(gimp-image-resize image title-width title-height 0 0)
		
		(script-fu-drop-shadow image layer 12 12 4 '(0 0 0) 0 0)
		;Need to do this to make the text layer into a normal layer so that drop shadow script works, so do any text changes before this line
		(gimp-item-set-name (car (gimp-image-get-layer-by-name image "Drop Shadow")) "fake-shadow")
		
		(gimp-item-set-name layer "main layer")
		(gimp-image-set-active-layer image layer)
		(gimp-image-raise-item-to-top image layer)
		(gimp-image-select-item image 0 layer)
		
		
		;Create gradient
		;Select original title text using “Alpha to selection” (i.e. don’t select any outlines or shadow)
		;Create new layer which is filled with transparency
		;Use gradient tool
			;First color is target color with V = 50% (darken it)
			;Middle color is target color
			;Last color is target color with saturation changed to 30%
			;Now change the midpoints according to the diagram below (red arrows are midpoints, circles are stop points)
			;Send to top of layers
		
		(gimp-image-select-item image 0 layer)
		(gimp-context-set-gradient title-gradient)
		
		(gimp-drawable-edit-gradient-fill layer GRADIENT-LINEAR 0 FALSE 0 0 TRUE 0 0 0 title-size)
		(gimp-drawable-hue-saturation layer 0 0 0 50 0)

		
		;Create first outline
		;Select all title text using “Alpha to selection”
		(gimp-image-select-item image 0 layer)
		;Create new layer which is filled with transparency
		(set! title-first-outline-layer (car (gimp-layer-new image title-width title-height 1 "title-first-outline-layer" 100 0)))
		(gimp-image-insert-layer image title-first-outline-layer 0 (car (gimp-image-get-item-position image layer)))
		(gimp-image-set-active-layer image title-first-outline-layer)
		;Grow selection by 6 pixels
		;Fill with grey using bucket tool (color is black, with V = 25%)
		(if (string=?  "TRUE" outline-dark) (gimp-context-set-foreground '(61 61 61)) (if (string=?  "MID" outline-dark) (gimp-context-set-foreground '(100 100 100)) (gimp-context-set-foreground '(229 229 229))))
		(gimp-context-set-stroke-method 0)
		(gimp-context-set-line-width border-size)
		(gimp-context-set-line-join-style 1)
		(gimp-drawable-edit-stroke-selection title-first-outline-layer)
		;Send to back of layers
		
		;Create 2nd outline
		;Use current selection (from previous step)
		(gimp-image-select-item image 0 layer)
		;Create new layer which is filled with transparency
		(set! title-second-outline-layer (car (gimp-layer-new image title-width title-height 1 "title-second-outline-layer" 100 0)))
		(gimp-image-insert-layer image title-second-outline-layer 0 (car (gimp-image-get-item-position image layer)))
		(gimp-image-set-active-layer image title-second-outline-layer)
		;Grow selection by another 6 pixels
		;Fill with grey using bucket tool (color is black, with V = 15%)
		(if (string=?  "TRUE" outline-dark) (gimp-context-set-foreground '(45 45 45)) (if (string=?  "MID" outline-dark) (gimp-context-set-foreground '(74 74 74)) (gimp-context-set-foreground '(140 140 140))))
		
		(gimp-context-set-line-width (* border-size 2))
		(gimp-drawable-edit-stroke-selection title-second-outline-layer)
		;Send to back of layers
		
		
		;Create drop shadow
		;Use current selection (from previous step)
		;Create new layer which is filled with transparency
		;Do not grow selection
		;Fill with black using bucket tool
		;Apply Gaussian filter with blur radius = 2
		;Shift down and right roughly 6 pixels
		;Send to back of layers
		
		(gimp-image-select-item image 0 title-second-outline-layer)
		(script-fu-drop-shadow image title-second-outline-layer 6 6 2 '(0 0 0) 100 0)
		;set the name so we don't have duplicates
		(gimp-item-set-name (car (gimp-image-get-layer-by-name image "Drop Shadow")) "text-layer-shadow")

		;Delete / remove / hide the vanilla text so any kind of 1 pixel border goes away


		
		(gimp-image-undo-group-end image)
		
		; save xcf before merging down
		
		;(set! layer   (car (gimp-image-merge-visible-layers image 1) ) )
		(list image layer)
    )
  )

