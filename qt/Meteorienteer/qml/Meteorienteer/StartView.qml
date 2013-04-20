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
        Image {
            width: (bgImage.width - 80) / 3;
            height: width
            source: "qrc:/gfx/buttonNormal.png";
            Text {
                color: "#ff0000"
                text: qsTr("New Observation")
                wrapMode: Text.WordWrap
                width: parent.width
                anchors.centerIn: parent
                font.pixelSize: 30
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    createObservation()
                }
                onPressed: {
                    parent.source = "qrc:/gfx/buttonHover.png";
                }
                onReleased: {
                    parent.source = "qrc:/gfx/buttonNormal.png";
                }
            }
        }

        Image {
            width: (bgImage.width - 80) / 3;
            height: width
            source: "qrc:/gfx/buttonNormal.png";
            Text {
                color: "#ff0000"
                text: qsTr("Meteor Info")
                wrapMode: Text.WordWrap
                width: parent.width
                anchors.centerIn: parent
                font.pixelSize: 30
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    showInfo()
                }
                onPressed: {
                    parent.source = "qrc:/gfx/buttonHover.png";
                }
                onReleased: {
                    parent.source = "qrc:/gfx/buttonNormal.png";
                }
            }
        }


        Image {
            width: (bgImage.width - 80) / 3;
            height: width
            source: "qrc:/gfx/buttonNormal.png";
            Text {
                color: "#ff0000"
                text: qsTr("Observation History");
                wrapMode: Text.WordWrap
                width: parent.width
                anchors.centerIn: parent
                font.pixelSize: 30
            }
            MouseArea {
                anchors.fill: parent
                onClicked: {
                    showHistory()
                }
                onPressed: {
                    parent.source = "qrc:/gfx/buttonHover.png";
                }
                onReleased: {
                    parent.source = "qrc:/gfx/buttonNormal.png";
                }
            }
        }
    }
}
