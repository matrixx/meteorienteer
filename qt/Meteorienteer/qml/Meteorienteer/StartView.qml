// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    signal createObservation;
    signal showInfo;
    signal showHistory;

    color: "black"
    anchors.fill: parent
    Image {
        id: bgImage
        source: "qrc:/gfx/linnunrata_N9.jpg"
        anchors.fill: parent
    }
    Row {
        anchors.verticalCenter: parent.verticalCenter
        anchors.left: parent.left
        anchors.leftMargin: 20
        spacing: 20
        MeteorButton {
            width: (bgImage.width - 80) / 3
            text: qsTr("New Observation")
            onClicked: {
                createObservation();
            }
        }
        MeteorButton {
            width: (bgImage.width - 80) / 3;
            text: qsTr("Meteor Info")
            onClicked: {
                showInfo();
            }
        }
        MeteorButton {
            width: (bgImage.width - 80) / 3;
            text: qsTr("Observation History");
            onClicked: {
                showHistory()
            }
        }
    }
}
