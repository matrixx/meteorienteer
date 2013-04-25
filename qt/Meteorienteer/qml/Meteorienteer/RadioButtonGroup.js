var buttonList = [];
var selectedIndex = 0;

function populate() {
    var list = mgr.values();
    if (list.length > 0) {
        var component = Qt.createComponent("ExclusiveRadioButton.qml");
        for (var i = 0; i < list.length; ++i) {
            var button = component.createObject(radioColumn);
            button.index = i;
            button.text = list[i];
            if (i === 0) {
                button.checked = true;
            } else {
                button.checked = false;
            }
            button.selected.connect(onSelectionChanged());
            buttonList.push(button);
        }
    }
}

function onSelectionChanged(index) {
    if (index !== selectedIndex) {
        selectedIndex = index;
        for (var i = 0; i < buttonList.length; ++i) {
            if (i !== selectedIndex) {
                console.debug("unchecking index:" + i);
                buttonList[i].unCheck();
            }
        }
    }
}

function save() {
    var validationError = mgr.setValueIndex(selectedIndex);
    if (validationError === "") {
        console.debug("successfully submitted value: " + selectedIndex);
        radioButtonField.saved(true);
    } else {
        console.debug("validation error: " + validationError);
        radioButtonField.saved(false);
    }
}
