The icons of this tool has been obtained from:
https://www.iconexperience.com/g_collection/


Keyboard Shortcuts:

== SkeletonEditor/PEditor
CTRL+U	->	Undo
CTRL+R	->	Redo
Mouse Wheel -> Zoom In/Out (fast) Model
CTRL+Mouse Wheel -> Zoom In/Out (slow) Model

Left Button			->	(Rotate)
Right Button		->	(Zoom In/Out)
Mouse Middle Button ->	(Panning)


== SkeletonEditor only
CTRL+S			->	Save Skeleton (direct path from where it has been opened)
CTRL+W			->	Save Animation (direct path from where it has been opened)
CTRL+D			->	Open Char.LGP Database Window (if path found and set)
CTRL+Home		->	Reset Camera
CTRL+Up/Down	->	Rotate alpha with keyboard
CTRL+Left/Right	->	Rotate beta with keyboard
DELETE			->	Remove selected bone/piece
Double-click Left Button on Texture -> Open Texture Coordinates(UVs) Viewer (if there is texture active)

== PEditor only
+/-			->	More/Less Brightness of the colors
R 			->	Reset Brightness of the colors
H           ->  You can Hide/Show the selected group
CTRL+Home	->	Reset Plane
SHIFT+Left Mouse Click	->	Gets polygon color and sets in palette
(this only works in "Paint Polygon" feature)
ESCAPE		->	You close the window without applying/commiting changes


==== Special Features
== Skeleton Editor
* Attaching Weapon
You can attach the weapon model (in Battle Models with Main PC models and Sephirot only)
to the model in Skeleton Editor. This feature was in KimeraVB6.
For this:
1.- You push "Compute Attached weapon position" button
2.- Select with Mouse Right/Left button the bone you want to attach it
    (middle or end part of the bone).
3.- If you want to cancel this special feature you can press "ESCAPE" key.


== PEditor
* Drag&Drop new color to color table
You can Drag&Drop the color selected in PEditor (right of checkbox "Palette Mode")
in the Palette when "Palette mode" is checked (you can see the palette of colors of the model).
And you can set this color from the gradient colors preview or selecting with the
horizontal scroll bars a new color. This is a new feature of KimeraCS.


* Use the three buttons of the mouse to select the default action/event you want for the button.
The button you use when pressing the action (Paint, Cut Edge, Erase polygon...) will be the
default button for that action. This feature was in KimeraVB6
By default, these are the actions:
Red background = Rotate (Left Mouse button)
Blue background = Zoom In/Out (Right Mouse button)
Green background = Panning (Middle Mouse button)

* New Polygon
You need to pick 3 vertex to create new polygon. There is a counter in the button.
You can reset the counter of the vertices picked with "N" key 
(while button "New polygon" is pushed). This feature is also in KimeraVB6, 
except the picked vertices counter.
