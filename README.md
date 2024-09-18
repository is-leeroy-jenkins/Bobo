![ScreenShot](https://i.ibb.co/pQgHy39/App-ML.png)

# Optical Text Recognition Trainer in ML.Net

Bobo is single page WPF application that can train a model for optical text recognition using 6 different machine learning algorithms viz SDCA maximum entropy, SDCA non-calibrated, Limited memory BFGS, Naive Bayes, Light gradient boosting machine, and Tensorflow. The application allows the user to choose the fonts available locally on the user's machine, the application generates optical data for selected characters which are then used as training data for model training. The application allows users to set minimum and maximum rotations for generating optical character data for training.
The most efficient algorithm for character recognition is surprisingly Light gradient boosting machine, not Tensorflow, surprisingly character recognition accuracy of the trained model is far far better than google's tesseract engine. No claim is meaningful without supporting data:

![Statistics](https://i.ibb.co/rHScR48/Accuracy.png "Accuracy")

## Short tutorial
* On left panel you can view and browse fonts available on your system. 
* Hit enter to add font with current settings to cart on right panel.
* Use left and right arrow key for next and previous font selected.
* Select the settings for training generation in center panel.
* Press F5 for training model with selected engine/algorithm. 
* Progress of training can be checked on lower right corner
* Press F9 to test the selected font panel

## Shortcuts 
* **ENTER** - Add font to cart
* **DELETE** - Delete font shown in font panel from cart.
* **FORWARD ARROW KEY** - Show next system font in font panel.
* **BACKWARD ARROW KEY** - Show previous system font in font panel.
* **Shift + DELETE** - Empty font cart.
* **F5** - Train model for fonts in cart for selected engine.
* **F8** - Test Accuracy (Not Recommended - inaccurate calculation in some engines)
* **F9** - Test selected model against selected font in font panel for current fonts settings.

## MIT LICENSE

Copyright (c) 2021-2024 Terry D. Eppler

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
