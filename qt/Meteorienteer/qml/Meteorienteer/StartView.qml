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
        source: "qrc:/gfx/bg.jpg"
        anchors.margins: 20
        anchors.fill: parent
    }
    Row {
        x: 40
        y: 40
        spacing: 20
        Rectangle {
            width: bgImage.width / 3 - 40;
            height: width
            gradient: Gradient {
                GradientStop { color: "#333333"; position: 0.0 }
                GradientStop { color: "#444444"; position: 0.2 }
                GradientStop { color: "#444444"; position: 0.8 }
                GradientStop { color: "#333333"; position: 1.0 }
            }
            Text {
                color: "#ff0000"
                text: qsTr("New Observation")
                wrapMode: Text.WordWrap
                anchors.fill: parent
                anchors.margins: 10
                font.pixelSize: 28
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    createObservation()
                }
            }
        }


        Rectangle {
            width: bgImage.width / 3 - 40;
            height: width
            gradient: Gradient {
                GradientStop { color: "#333333"; position: 0.0 }
                GradientStop { color: "#444444"; position: 0.2 }
                GradientStop { color: "#444444"; position: 0.8 }
                GradientStop { color: "#333333"; position: 1.0 }
            }
            Text {
                color: "#ff0000"
                text: qsTr("Meteor Info")
                wrapMode: Text.WordWrap
                anchors.fill: parent
                anchors.margins: 10
                font.pixelSize: 28
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    showInfo()
                }
            }
        }


        Rectangle {
            width: bgImage.width / 3 - 40;
            height: width
            gradient: Gradient {
                GradientStop { color: "#333333"; position: 0.0 }
                GradientStop { color: "#444444"; position: 0.2 }
                GradientStop { color: "#444444"; position: 0.8 }
                GradientStop { color: "#333333"; position: 1.0 }
            }
            Text {
                color: "#ff0000"
                text: qsTr("Observation History");
                wrapMode: Text.WordWrap
                anchors.fill: parent
                anchors.margins: 10
                font.pixelSize: 28
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    showHistory()
                }
            }
        }
    }
}
