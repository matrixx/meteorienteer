// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import QtMultimediaKit 1.1

Rectangle {
    signal measurementsSaved
    anchors.fill: parent
    color: "black"

    LocationDataProvider {
        id: measurements
    }
    onVisibleChanged: {
        if (visible) {
            measurements.startMeasure();
        } else {
            measurements.stopMeasure();
        }
    }
    Camera {
        x: 0
        y: 0
        width: parent.width
        height: parent.height
        captureResolution: "900x506" // 3:2
        Column {
            id: pageContent
            spacing: 16
            Text {
                wrapMode: Text.WordWrap
                text: qsTr("Accelerometer reading: %1 | %2 | %3").arg(measurements.accelx).arg(measurements.accely).arg(measurements.accelz)
                color: "#0000ff"
            }
            Text {
                wrapMode: Text.WordWrap
                text: qsTr("Compass azimuth in degrees: %1").arg(measurements.azimuth)
                color: "#0000ff"
            }
            Text {
                wrapMode: Text.WordWrap
                text: qsTr("GPS coordinates: %1 | %2").arg(measurements.latitude).arg(measurements.longitude)
                color: "#0000ff"
            }
        }
    }
    MouseArea {
        anchors.fill: parent
        onClicked: {
            measurementsSaved();
        }
    }
}
