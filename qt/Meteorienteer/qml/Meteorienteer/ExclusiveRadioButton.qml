// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import com.nokia.meego 1.0

Rectangle {
    property alias checked: rb.checked
    property alias text: buttonLabel.text
    property int index: 0;
    signal selected(int index)
    width: parent.width
    height: Math.max(buttonLabel.height, 100);
    color: "black"
    border.color: "white"
    border.width: 1
    RadioButton {
        id: rb
        checked: false
        anchors.left: parent.left
        anchors.verticalCenter: parent.verticalCenter
        onCheckedChanged: {
            if (checked) {
                parent.selected(index)
            }
        }
    }
    Text {
        id: buttonLabel
        anchors.margins: 20
        anchors.left: rb.right
        anchors.top: parent.top
        width: 380 - rb.width
        text: mgr.label
        wrapMode: Text.WordWrap
        color: "white"
        font.pixelSize: 30
    }

    function unCheck() {
        rb.checked = false;
    }
}


