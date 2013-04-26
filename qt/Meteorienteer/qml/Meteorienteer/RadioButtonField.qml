// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import "RadioButtonGroup.js" as Group

Rectangle {
    id: radioButtonField
    color: "black";
    signal saved(bool success)
    Text {
        id: label
        width: 400
        anchors.margins: 20
        anchors.top: parent.top
        anchors.left: parent.left
        text: mgr.label
        color: "white"
        font.pixelSize: 30
        wrapMode: Text.WordWrap
    }

    Flickable {
        anchors.left: label.right
        anchors.top: parent.top
        contentWidth: radioParent.width
        contentHeight: radioParent.height
        width: 400
        height: parent.height
        Rectangle {
            id: radioParent
            width: 400
            height: childrenRect.height
            radius: 15
            border.color: "white"
            border.width: 1
            Component.onCompleted: {
                Group.populate();
            }
            Column {
                id: radioColumn
                width: parent.width
            }
        }
    }

    function save() {
        Group.save();
    }
}
