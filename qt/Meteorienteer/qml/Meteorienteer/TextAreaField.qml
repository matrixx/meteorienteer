// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import com.nokia.meego 1.1

Rectangle {
    color: "black";
    signal saved(bool success)
    Text {
        id: label
        anchors.margins: 20
        anchors.top: parent.top
        anchors.horizontalCenter: parent.horizontalCenter
        text: mgr.label
        color: "white"
        font.pixelSize: 30
    }
    TextArea {
        id: textArea
        text: mgr.prefilled
        anchors.horizontalCenter: parent.horizontalCenter
        anchors.top: label.bottom
        anchors.topMargin: 20
        anchors.bottom: parent.bottom
        anchors.bottomMargin: 20
        wrapMode: Text.WordWrap
        width: parent.width
    }

    function save() {
        var validationError = mgr.setValue(textArea.text);
        if (validationError === "") {
            console.debug("successfully submitted value: " + textArea.text);
            saved(true);
        } else {
            console.debug("validation error: " + validationError);
            saved(false);
        }
    }
}
