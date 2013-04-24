// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import "FieldHandler.js" as FieldHandler

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
        MeteorButton {
            width: 220;
            text: qsTr("Previous")
            anchors.top: parent.top
            anchors.bottom: parent.bottom
            anchors.left: parent.left
            onClicked: {
                loadPreviousField();
            }
        }
        MeteorButton {
            width: 220;
            text: qsTr("Cancel")
            anchors.top: parent.top
            anchors.bottom: parent.bottom
            anchors.horizontalCenter: parent.horizontalCenter
            onClicked: {
                reset();
            }
        }
        MeteorButton {
            width: 220;
            text: qsTr("Next");
            anchors.top: parent.top
            anchors.bottom: parent.bottom
            anchors.right: parent.right
            onClicked: {
                FieldHandler.save();
            }
        }
    }

    function init() {
        loadNextField();
    }

    function onSubmitted(success) {
        if (success) {
            formSubmitSucceeded();
        } else {
            formSubmitFailed();
        }
    }
    function loadNextField() {
        if (mgr.next()) {
            FieldHandler.loadCurrentField();
        } else {
            mgr.submit(); // show overview and submit
            formSubmitSucceeded();
        }
    }
    function loadPreviousField() {
        if (mgr.previous()) {
            FieldHandler.loadCurrentField();
        } else {
            reset();
        }
    }
    function onSaved(success) {
        if (success) {
            console.debug("success in save, loading next");
            loadNextField();
        } else {
            console.debug("failed in save, this should pop up error screen, but not implemented yet");
        }
    }
}
