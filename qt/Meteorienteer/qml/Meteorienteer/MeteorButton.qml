// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Image {
    width: 200;
    height: width
    property alias text: buttonText.text
    signal clicked();
    source: "qrc:/gfx/buttonNormal.png";

    Text {
        id: buttonText
        color: "#ffffff"
        text: "MeteorButton"
        wrapMode: Text.WordWrap
        width: parent.width - 40
        anchors.centerIn: parent
        font.pixelSize: 30
    }
    MouseArea {
        anchors.fill: parent
        onClicked: {
            parent.clicked();
        }
        onPressed: {
            parent.source = "qrc:/gfx/buttonActive.png";
        }
        onReleased: {
            parent.source = "qrc:/gfx/buttonNormal.png";
        }
    }
}

