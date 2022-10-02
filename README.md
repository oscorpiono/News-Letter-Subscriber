# News-Letter-Subscriber
 
This app allows users to manipulate web actions just by coding (Like IMACROS but more gestion available and supports new versions of browsers) it's made just for newsletter subscriptions.
You can give it more power if you know how to code !!



# How to Code ?

This is an example :


this example helps to subscribe a certain email specified in a text file in wb-fernstudium.de's newsletter.
https://www.wb-fernstudium.de/	SHADOW|#usercentrics-root,CLICK:XPATH|button[data-testid="uc-accept-all-button"]:1	CLICK:CSS|label[for="newsletter_anrede_Frau"]:1	INPUT:ID|inputVorname:1	INPUT:ID|inputNachname:1	EMAIL:ID|email:1	CLICK:CSS|input#formKnNewsAjaxButton.btn.btn-primary:1

# Actions :
SHADOW : Can access shadow-root elements in html and do actions in forms on them.
         it takse an ID as a selector.
         
CLICK : CLICKS THE SELECTED ELEMENT
        it take the selector for the element as a property.
        
EMAIL : Uses current email in the imported emails file
        it take the selector for the element as a property.
        
INPUT : Sends auto-generated text and type it inside the input selected
        it take the selector for the element as a property.
        

# Selectors:
All selectors are available :

XPATH: selects element by XPath.

CSS: selects element by CSS Selector.

ID: selects element by ID.

TAG: selects element by TAG.

NAME: selects element by NAME.


Formula : 
[SHADOW ELEMENT IF NEEDED]|[ID OF THE PARENT OF SHADOW ROOT],[ACTION TODO]:[SELECTOR TYPE]|[SELECTOR VALUE]:[(NUMBER OF ELEMENTS TO APPLY ACTION ON) or (YOU CAN USE "pos:[ELEMENT POSITION]" to only apply to the element in that position)].

(-NOTE: Important to specify those three values : [ACTION TODO]:[SELECTOR TYPE]|[SELECTOR VALUE]:[(NUMBER OF ELEMENTS TO APPLY ACTION ON) or (YOU CAN USE "pos:[ELEMENT POSITION]" to only apply to the element in that position)]).

Now let's see how to write actions or whatever.

let's see the more advanced one which is : SHADOW|#usercentrics-root,CLICK:XPATH|button[data-testid="uc-accept-all-button"]:1.

SHADOW : which helps to access shadow root parent element.

"|" : it helps the app understand that the ID of the shadow root parent element is the string after it (between "|" and ",").

CLICK : the action that should be made on an element.

":" : it helps the app understand that the selector of the element is the string after it (between ":" and ":").

XPATH : the type of selector used.

"|" : it helps the app understand that the value of the selector of the element is the string after it (between "|" and ":").

button[data-testid="uc-accept-all-button"] : the value of selector used.

":" : it helps the app understand that the value after it could be either the number of elements to apply action on or the position of the elment to apply action on in case multiple elements with same selector exists. (:1) or (:pos:1)




# Conclusion:
This was just a madde for fun app "No judges" ;).
