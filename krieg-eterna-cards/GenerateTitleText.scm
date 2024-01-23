  (script-fu-register
    "script-fu-gen-text"                    ;function name
    "Gen Text"                                  ;menu label
    "Creates Title Text"              ;description
    "Andrew Wollack"                             ;author
    "copyright 1997, Michael Terry;\
      2009, the GIMP Documentation Team"        ;copyright notice
    "October 27, 1997"                          ;date created
    ""                                      ;image type that the script works on
    SF-STRING      "Text"          "Frost"   					;titleText 		need
	SF-GRADIENT    "Gradient" 	   "Weather"					;title-gradient		need
	SF-FILENAME    "FILENAME"     (string-append "C:\\Users\\Drew\\Documents\\Gimp\\out\\" "") 	;out-folder
	SF-FILENAME    "FILENAME"     "Frost" 	;out-file
	SF-TOGGLE      "OutlineDark"       TRUE 	;outline dark (true) or Light(false flag

  )
  (script-fu-menu-register "script-fu-gen-text" "<Image>/File/Create/Text")

  
  (define (script-fu-gen-text titleText title-gradient out-folder out-file outline-dark)
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
		(titleFont 	"Gargouille")
		(titleFontSize 500)
		(inBufferAmount 108)
		(yfact 20)
		(boxFac)
        (image)
        (image
                  (car
					(gimp-image-new 10 10 0)
                  )
        )
        (text)             ;a declaration for the text
        (buffer)           ;create a new layer for the image
		(title-size)
        (layer)
		(title-back-layer)
		(title-first-outline-layer)
		(title-second-outline-layer)
		(title-gradient-layer)
		(merge-layer)
      ) ;end of our local variables
	  
	  ;;Cropping
	  ;Ratio is h px with 36px margin
	  (gimp-message  titleText)
	  (gimp-message  out-folder)
	  (gimp-message  out-file)
	  
	  (set! image-width   (car (gimp-image-width  image) ) )
	  (set! image-height  (car (gimp-image-height image) ) )
	  

	  
	  (gimp-image-scale image 2466 3366)

	  
	  
      
	  (set! image-width   (car (gimp-image-width  image) ) )
	  (set! image-height  (car (gimp-image-height image) ) )
	  
	  (set! layer  (car
			  (gimp-layer-new
				image
				image-width
				image-height
				RGB-IMAGE
				"layer 1"
				0
				LAYER-MODE-NORMAL
			  )
		))

		  
	(gimp-image-insert-layer image layer 0 0)

	(set! buffer inBufferAmount)
	  
	  ;;Title  formatting
	  (gimp-message "Title")
	  (set! title-size (/ image-width 5.5)) 
	  ;Make vanilla title text
      (set! text
                    (car
                          (gimp-text-fontname
                          image layer
                          0 0
                          titleText
                          0
                          TRUE
                          title-size PIXELS
                          titleFont)
                      )
        )	

		
		(set! title-width   (car (gimp-drawable-width  text) ) )
		(set! title-height  (car (gimp-drawable-height text) ) )
		(set! title-height (+ title-height buffer buffer) )
		(set! title-width  (+ title-width  buffer buffer) )
		(gimp-layer-resize layer title-width title-height 0 0)
		(gimp-layer-set-offsets text (* 2.35 buffer) (* 2.25 buffer))
		(gimp-text-layer-set-letter-spacing text 10)
		
		(script-fu-drop-shadow image text 12 12 4 '(0 0 0) 0 0)
		(gimp-floating-sel-to-layer text)
		;Need to do this to make the text layer into a normal layer so that drop shadow script works, so do any text changes before this line
		(gimp-item-set-name (car (gimp-image-get-layer-by-name image "Drop Shadow")) "fake-shadow")
		
		(gimp-item-set-name text "title-text")
		(gimp-image-set-active-layer image text)
		(gimp-image-raise-item-to-top image text)
		(gimp-image-select-item image 0 text)
		
		
		;Create gradient
		(gimp-message "Title Gradient")
		;Select original title text using “Alpha to selection” (i.e. don’t select any outlines or shadow)
		;Create new layer which is filled with transparency
		;Use gradient tool
			;First color is target color with V = 50% (darken it)
			;Middle color is target color
			;Last color is target color with saturation changed to 30%
			;Now change the midpoints according to the diagram below (red arrows are midpoints, circles are stop points)
			;Send to top of layers
		(gimp-image-undo-disable image)
		(gimp-context-push)
		
		(gimp-image-select-item image 0 text)
		(gimp-context-set-gradient title-gradient)
		
		(gimp-message (number->string image-height))
		(gimp-message (number->string (* title-height (/ 7 9))))
		(gimp-message (number->string (string-length titleText)))
		(gimp-message (number->string (- (* 5 (string-length titleText)) 10)))
		(gimp-drawable-edit-gradient-fill text GRADIENT-LINEAR 0 FALSE 0 0 TRUE 0 0 0 title-size)
		(gimp-drawable-hue-saturation text 0 0 0 50 0)

		
		;Create first outline
		(gimp-message "Title First Outline")
		;Select all title text using “Alpha to selection”
		(gimp-image-select-item image 0 text)
		;Create new layer which is filled with transparency
		(set! title-first-outline-layer (car (gimp-layer-new image title-width title-height 1 "title-first-outline-layer" 100 0)))
		(gimp-layer-set-offsets title-first-outline-layer (* 2 buffer) (* 2 buffer))
		(gimp-image-insert-layer image title-first-outline-layer 0 (car (gimp-image-get-item-position image layer)))
		(gimp-image-set-active-layer image title-first-outline-layer)
		;Grow selection by 6 pixels
		;Fill with grey using bucket tool (color is black, with V = 25%)
		(if (string=?  "TRUE" outline-dark) (gimp-context-set-foreground '(61 61 61)) (if (string=?  "MID" outline-dark) (gimp-context-set-foreground '(100 100 100)) (gimp-context-set-foreground '(229 229 229))))
		(gimp-context-set-stroke-method 0)
		(gimp-context-set-line-width 12)
		(gimp-context-set-line-join-style 1)
		(gimp-drawable-edit-stroke-selection title-first-outline-layer)
		;Send to back of layers
		
		;Create 2nd outline
		(gimp-message "Title Second Outline")
		;Use current selection (from previous step)
		(gimp-image-select-item image 0 text)
		;Create new layer which is filled with transparency
		(set! title-second-outline-layer (car (gimp-layer-new image title-width title-height 1 "title-second-outline-layer" 100 0)))
		(gimp-layer-set-offsets title-second-outline-layer (* 2 buffer) (* 2 buffer))
		(gimp-image-insert-layer image title-second-outline-layer 0 (car (gimp-image-get-item-position image layer)))
		(gimp-image-set-active-layer image title-second-outline-layer)
		;Grow selection by another 6 pixels
		;Fill with grey using bucket tool (color is black, with V = 15%)
		(if (string=?  "TRUE" outline-dark) (gimp-context-set-foreground '(45 45 45)) (if (string=?  "MID" outline-dark) (gimp-context-set-foreground '(74 74 74)) (gimp-context-set-foreground '(140 140 140))))
		
		(gimp-context-set-line-width 24)
		(gimp-drawable-edit-stroke-selection title-second-outline-layer)
		;Send to back of layers
		
		
		;Create drop shadow
		(gimp-message "Title Drop Shadow")
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
		(gimp-message "merging")
		(gimp-item-set-name (car (gimp-image-get-layer-by-name image "Drop Shadow")) "text-layer-shadow")

		;Delete / remove / hide the vanilla text so any kind of 1 pixel border goes away
		
		
		(gimp-context-pop)
		(gimp-image-undo-enable image)


		
		
		; save xcf before merging down
		
		(set! layer   (car (gimp-image-merge-visible-layers image 1) ) )
		(plug-in-autocrop 0 image layer)

		(file-png-save 1 image layer (string-append (string-append out-folder "/png/") (string-append out-file ".png")) (string-append (string-append out-folder "/png/") (string-append out-file ".png")) 0 0 1 0 0 1 1)
		(list image layer text)
    )
  )

