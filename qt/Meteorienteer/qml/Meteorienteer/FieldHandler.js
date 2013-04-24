var currentField = 0;

function loadCurrentField() {
    var fieldComponent;
    var fieldObject;
    fieldComponent = Qt.createComponent(mgr.type);
    fieldObject = fieldComponent.createObject(formSpace, { "anchors.fill" : formSpace });
    console.debug("created new object");
    if (currentField !== 0) {
        console.debug("distroying old object");
        currentField.destroy();
    }
    currentField = fieldObject;
    currentField.saved.connect(formView.onSaved);
    console.debug("loaded component");
}

function reset() {
    formCanceled();
    currentField.destroy();
    currentField = 0;
    mgr.reset();
}

function save() {
    currentField.save();
}
