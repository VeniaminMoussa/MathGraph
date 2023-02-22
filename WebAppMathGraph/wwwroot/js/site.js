// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function autocomplete(inp, arr) {
    /*the autocomplete function takes two arguments,
    the text field element and an array of possible autocompleted values:*/
    var currentFocus;
    /*execute a function when someone writes in the text field:*/
    inp.addEventListener("input", autoCompleteInputHandler);

    function autoCompleteInputHandler(e) {
        var a, b, i, val = this.value;
        /*close any already open lists of autocompleted values*/
        closeAllLists();
        if (!val) { return false; }
        currentFocus = -1;
        /*create a DIV element that will contain the items (values):*/
        a = document.createElement("DIV");
        a.setAttribute("id", this.id + "autocomplete-list");
        a.setAttribute("class", "autocomplete-items");
        /*append the DIV element as a child of the autocomplete container:*/
        this.parentNode.appendChild(a);
        /*execute a function presses a key on the keyboard:*/
        inp.addEventListener("keydown", autoCompleteKeyDownHandler);
        /*for each item in the array...*/
        for (i = 0; i < arr.length; i++) {

            var words = val.split(/[\s]+/);
            var word = words[words.length - 1];
            var index = inp.value.lastIndexOf(word);

            if (val.charAt(val.length - 1).match(/[\s]+/)) {
                index = val.length;
            }

            /*check if the item starts with the same letters as the text field value:*/
            if (arr[i].substr(0, word.length).toUpperCase() == word.toUpperCase()) {
                /*create a DIV element for each matching element:*/
                b = document.createElement("DIV");
                /*make the matching letters bold:*/
                b.innerHTML = "<strong>" + arr[i].substr(0, word.length) + "</strong>";
                b.innerHTML += arr[i].substr(word.length);
                /*insert a input field that will hold the current array item's value:*/
                b.innerHTML += "<input type='hidden' value='" + arr[i] + "'>";
                /*execute a function when someone clicks on the item value (DIV element):*/
                b.addEventListener("click", function (e) {
                    /*insert the value for the autocomplete text field:*/

                    var output = [inp.value.slice(0, index), this.getElementsByTagName("input")[0].value].join('');
                    inp.value = output;

                    /*close the list of autocompleted values,
                    (or any other open lists of autocompleted values:*/
                    closeAllLists();
                });
                a.appendChild(b);
            }
        }

        if (a.childElementCount == 0) {
            inp.removeEventListener("keydown", autoCompleteKeyDownHandler);
        }
    }

    function autoCompleteKeyDownHandler(event) {
        var x = document.getElementById(this.id + "autocomplete-list");
        if (x) x = x.getElementsByTagName("div");
        if (event.key == "ArrowDown") {
            /*If the arrow DOWN key is pressed,
            increase the currentFocus variable:*/
            event.preventDefault();
            currentFocus++;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (event.key == "ArrowUp") { //up
            /*If the arrow UP key is pressed,
            decrease the currentFocus variable:*/
            event.preventDefault();
            currentFocus--;
            /*and and make the current item more visible:*/
            addActive(x);
        } else if (event.key == "Enter") {
            if (currentFocus > -1) {
                /*and simulate a click on the "active" item:*/
                event.preventDefault();
                if (x) x[currentFocus].click();
            }
        }
    }

    function addActive(x) {
        /*a function to classify an item as "active":*/
        if (!x) return false;
        /*start by removing the "active" class on all items:*/
        removeActive(x);
        if (currentFocus >= x.length) currentFocus = 0;
        if (currentFocus < 0) currentFocus = (x.length - 1);
        /*add class "autocomplete-active":*/
        x[currentFocus].classList.add("autocomplete-active");
    }

    function removeActive(x) {
        /*a function to remove the "active" class from all autocomplete items:*/
        for (var i = 0; i < x.length; i++) {
            x[i].classList.remove("autocomplete-active");
        }
    }

    function closeAllLists(elmnt) {
        /*close all autocomplete lists in the document,
        except the one passed as an argument:*/
        var x = document.getElementsByClassName("autocomplete-items");
        for (var i = 0; i < x.length; i++) {
            if (elmnt != x[i] && elmnt != inp) {
                x[i].parentNode.removeChild(x[i]);
            }
        }
        inp.removeEventListener("keydown", autoCompleteKeyDownHandler);
    }

    /*execute a function when someone clicks in the document:*/
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
}