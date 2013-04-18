// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import QtMultimediaKit 1.1

Rectangle {
    anchors.fill: parent
    color: "black"
    LocationDataProvider {
        id: measurements
    }

    Row {
        Column {
            id: pageContent
            spacing: 16

            Text {
                wrapMode: Text.WordWrap
                text: qsTr("Accelerometer reading: %1 | %2 | %3").arg(measurements.accelx).arg(measurements.accely).arg(measurements.accelz)
                color: "#fff"
            }
            Text {
                wrapMode: Text.WordWrap
                text: qsTr("Compass azimuth in degrees: %1").arg(measurements.azimuth)
                color: "#fff"
            }
            Text {
                wrapMode: Text.WordWrap
                text: qsTr("GPS coordinates: %1 | %2").arg(measurements.latitude).arg(measurements.longitude)
                color: "#fff"
            }
        }
        Column {
            Camera {
                width: 800
                height: 480
            }
        }
    }
}
