// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    id: formView;
    property int direction: 0;
    signal formSubmitSucceeded;
    signal formSubmitFailed;
    signal formCanceled;

    color: "black"
    anchors.fill: parent
    Item {
        id: formSpace
        anchors.top: parent.top
        anchors.left: parent.left
        anchors.right: parent.right
        height: parent.height * 3 / 4;
    }
    Item {
        id: buttonSpace
        anchors.top: formSpace.bottom
        anchors.left: parent.left
        anchors.right: parent.right
        anchors.bottom: parent.bottom
    }

    function loadField() {
        if (mgr.next()) {
            var fieldComponent;
            var fieldObject;
            fieldComponent = Qt.createComponent(mgr.type);
            fieldObject = fieldComponent.createObject(formSpace, { "anchors.fill" : formSpace });
            console.debug("loaded component");
            //
        } else {
            // show overview and
        }
    }
}
