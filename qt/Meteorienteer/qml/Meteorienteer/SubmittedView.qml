// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    color: "black"
    property string service: "Taivaanvahti";
    Text {
        anchors.centerIn: parent
        font {
            pixelSize: 50
        }
        color: "white"
        wrapMode: Text.WordWrap
        width: parent.width - 20
        text: "You have successfully submitted your observation to " + service + "!";
    }
}
