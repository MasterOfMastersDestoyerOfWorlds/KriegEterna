  (script-fu-register
    "script-fu-compile-card"                    ;function name
    "Compile Card"                                  ;menu label
    "Creates a card, sized to fit\
      around the user's choice of text,\
      font, font size, and color."              ;description
    "Andrew Wollack"                             ;author
    "copyright 1997, Michael Terry;\
      2009, the GIMP Documentation Team"        ;copyright notice
    "October 27, 1997"                          ;date created
    ""                                      ;image type that the script works on
    SF-STRING      "Text"          "Frost"   					;titleText 		need
	SF-STRING      "Text"          "effect desc"   				;effect	    	need
	SF-STRING      "Text"          "flavor text"  				;flavor			need
	SF-GRADIENT    "Gradient" 	   "Weather"					;gradient		need
	SF-STRING  	   "Text" 	   	   "0"							;strength		need
	SF-FILENAME    "FILENAME" (string-append "C:\\Users\\Drew\\Documents\\Gimp\\cropped\\" "Frost.xcf") ;file
	SF-FILENAME    "FILENAME" (string-append "C:\\Users\\Drew\\Documents\\Gimp\\icons\\" "Weather.xcf") ;icon-file
	SF-FILENAME    "FILENAME" (string-append "C:\\Users\\Drew\\Documents\\Gimp\\out\\" "Frost.png") 	;out-file

  )
  (script-fu-menu-register "script-fu-compile-card" "<Image>/File/Create/Text")
  
  (define (script-fu-compile-card titleText effect flavor gradient strength file icon-file out-file)
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
		(effect-width 10)
		(effect-height 10)
		(strength-width 10)
		(strength-height 10)
		(flavor-width 10)
		(flavor-height 10)
		(titleFont 	"Gargouille")
		(descFont  	"Open Sans Semi-Bold")
		(flavorFont "Open Sans Semi-Bold Italic")
		(titleFontSize 500)
		(inBufferAmount 108)
		(yfact 20)
        (image)
        (image
                  (car
					(gimp-xcf-load 0 file file)
                  )
        )
        (text)             ;a declaration for the text
		(effect-text)
		(flavor-text)
        (buffer)           ;create a new layer for the image
		(title-size)
		(desc-size)
        (layer)
		(desc-layer)
		(icon-layer)
		(strength-layer)
		(strength-size)
		(strength-text)
      ) ;end of our local variables
	  
	  ;;Cropping
	  ;Ratio is h px with 36px margin
	  
	  (gimp-message "no variable TEST!")
	  
	  (set! image-width   (car (gimp-image-width  image) ) )
	  (set! image-height  (car (gimp-image-height image) ) )
	  
	  ;(set! crop-width   (- image-width offx))
	  ;(set! crop-height  (- image-height offy))
	  
	  ;(set! crop-width (* crop-width (/ (* 822 crop-height) (* 1122 crop-width))))
	  
	  
	  ;(gimp-image-crop image crop-width crop-height offx offy)
	  
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
		  
	  (set! desc-layer(car
		  (gimp-layer-new
			image
			image-width
			image-height
			RGB-IMAGE
			"layer 2"
			0
			LAYER-MODE-NORMAL
		  )
		))
		(set! strength-layer(car
		  (gimp-layer-new
			image
			image-width
			image-height
			RGB-IMAGE
			"layer 3"
			0
			LAYER-MODE-NORMAL
		  )
		))
		
		(set! icon-layer (car
			(gimp-file-load-layer 0 image icon-file)))
			  
	  (gimp-image-insert-layer image layer 0 0)
	  (gimp-image-insert-layer image desc-layer 0 0)
	  (gimp-image-insert-layer image icon-layer 0 0)
	  (gimp-image-insert-layer image strength-layer 0 0)
	  
	  (set! buffer inBufferAmount)
	  
	  ;;Icon Formatting
	  
	  (gimp-layer-scale icon-layer (/ image-width 5.5) (/ image-width 5.5) TRUE)
	  
	  (gimp-layer-set-offsets icon-layer (- (- image-width (/ image-width 5.5))  (* 2 buffer)) (* 2 buffer))
	  (gimp-selection-none image)
	  (gimp-image-select-item image 0 icon-layer)
	  ;(gimp-image-raise-item-to-top image text)
	  (script-fu-drop-shadow image icon-layer 12 12 4 '(0 0 0) 100 0)
	  (gimp-image-raise-item-to-top image icon-layer)
	  
	  
	  ;;Title  formatting
	  (gimp-message "Title")
	  (set! title-size (/ image-width 5.5)) 
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
		(gimp-layer-set-offsets text (* 2 buffer) (* 2 buffer))
		(gimp-image-select-item image 0 text)
		(script-fu-drop-shadow image text 12 12 4 '(0 0 0) 100 0)
		(gimp-floating-sel-to-layer text)
		(gimp-image-raise-item-to-top image text)

		;Render the gradient over the text
		(gimp-image-undo-disable image)
		(gimp-context-push)
		
		(gimp-image-select-item image 0 text)
		(gimp-context-set-gradient gradient)
		
		(gimp-message (number->string image-height))
		(gimp-message (number->string (* title-height (/ 7 9))))
		(gimp-message (number->string (string-length titleText)))
		(gimp-message (number->string (- (* 5 (string-length titleText)) 10)))
		(gimp-drawable-edit-gradient-fill text GRADIENT-LINEAR 0 FALSE 0 0 TRUE 0 0 (/ title-width (- (* 5 (string-length titleText)) 10)) (/ image-height 7.729))
		(gimp-selection-grow image 2)
		(gimp-selection-border image 2)
		(gimp-drawable-edit-fill text 1)
		
		(gimp-context-pop)
		(gimp-image-undo-enable image)
		
		
		
	  (gimp-message "Strength")
		;;Strength  formatting
		(if (>= (string-length strength) 1) (begin
		  (set! strength-size (/ image-width 6)) 
		  (set! strength-text
						(car
							  (gimp-text-fontname
							  image strength-layer
							  0 0
							  strength
							  0
							  TRUE
							  strength-size PIXELS
							  titleFont)
						  )
			)
		
		
		(set! strength-width   (car (gimp-drawable-width  strength-text) ) )
		(set! strength-height  (car (gimp-drawable-height strength-text) ) )
		(set! strength-height (+ strength-height buffer buffer) )
		(set! strength-width  (+ strength-width  buffer buffer) )
		(gimp-layer-resize strength-layer strength-width strength-height 0 0)
		(gimp-layer-set-offsets strength-text (- (- image-width (/ image-width 8))  buffer) (+ buffer (/ image-width 4)))
		
		(script-fu-drop-shadow image strength-text 12 12 4 '(0 0 0) 100 0)
		(gimp-floating-sel-to-layer strength-text)
		(gimp-image-raise-item-to-top image strength-text)

		;Render the gradient over the text
		(gimp-image-undo-disable image)
		(gimp-context-push)
		
		(gimp-image-select-item image 0 strength-text)
		(gimp-context-set-gradient gradient)
		(gimp-drawable-edit-gradient-fill strength-text GRADIENT-LINEAR 0 FALSE 0 0 TRUE 0 0 (/ strength-width yfact) (* strength-height (/ 7 9)))
		(gimp-selection-grow image 2)
		(gimp-selection-border image 2)
		(gimp-drawable-edit-fill strength-text 1)
		
		(gimp-context-pop)
		(gimp-image-undo-enable image))
		(begin
        (gimp-message "no strength!")))
		
		
		(gimp-message "Effect")
		;;Description  formatting
		(set! desc-size (/ image-width 30)) 
		(if (> (string-length effect) 1) (begin
		(set! effect-text
                    (car
                          (gimp-text-fontname
                          image desc-layer
                          0 0
                          effect
                          0
                          FALSE
                          desc-size PIXELS
                          descFont)
                      )
        )
		
		
		(set! effect-width   (car (gimp-drawable-width  effect-text) ) )
		(set! effect-height  (car (gimp-drawable-height effect-text) ) )
		(set! effect-height (+ effect-height buffer buffer) )
		(set! effect-width  (+ effect-width  buffer buffer) )
		(gimp-layer-set-offsets effect-text (+ buffer buffer) (- image-height (/ image-height 3)))
		(gimp-text-layer-set-color effect-text '(255 255 255))
		(script-fu-drop-shadow image effect-text 12 12 6 '(0 0 0) 100 0)
		(gimp-floating-sel-to-layer effect-text)
		(gimp-image-raise-item-to-top image effect-text)

		;Render the border
		(gimp-image-undo-disable image)
		(gimp-context-push)
		
		(gimp-image-select-item image 0 effect-text)
		;(gimp-drawable-edit-fill effect-text 1)
		(gimp-selection-grow image 2)
		(gimp-selection-border image 1)
		(gimp-selection-grow image 2)
		(gimp-drawable-edit-fill effect-text 0)
		
		(gimp-context-pop)
		(gimp-image-undo-enable image))
		(begin
        (gimp-message "no effect!")))
		
		(gimp-message "Flavor")
		(set! flavor-text
                    (car
                          (gimp-text-fontname
                          image desc-layer
                          0 0
                          flavor
                          0
                          FALSE
                          desc-size PIXELS
                          flavorFont)
                      )
        )
		
		
		
		(set! flavor-width   (car (gimp-drawable-width  flavor-text) ) )
		(set! flavor-height  (car (gimp-drawable-height flavor-text) ) )
		(set! flavor-height (+ flavor-height buffer buffer) )
		(set! flavor-width  (+ flavor-width  buffer buffer) )
		(gimp-layer-set-offsets flavor-text (+ buffer buffer) (- image-height (/ image-height 5)))
		(gimp-text-layer-set-color flavor-text '(255 255 255))
		(script-fu-drop-shadow image flavor-text 12 12 10 '(0 0 0) 100 0)
		(gimp-floating-sel-to-layer flavor-text)
		(gimp-image-raise-item-to-top image flavor-text)

		;Render the border
		(gimp-image-undo-disable image)
		(gimp-context-push)
		
		(gimp-image-select-item image 0 flavor-text)
		;(gimp-drawable-edit-fill flavor-text 1)
		(gimp-selection-grow image 2)
		(gimp-selection-border image 1)
		(gimp-selection-grow image 2)
		(gimp-drawable-edit-fill flavor-text 0)
		(gimp-context-pop)
		(gimp-image-undo-enable image)
		
		
		
		(set! layer   (car (gimp-image-merge-visible-layers image 1) ) )
		(file-png-save 1 image layer out-file out-file 0 9 1 0 0 1 1)
		(list image layer text effect-text flavor-text)
    )
  )