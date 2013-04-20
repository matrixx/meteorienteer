// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    anchors.fill: parent;
    signal directionSelected(int direction);
    property int direction: 0;
    Image {
        anchors.fill: parent
        id: name
        source: "http://farm9.staticflickr.com/8251/8507360785_5fde045b8c_c.jpg"
    }
}
