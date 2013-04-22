// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import QtMultimediaKit 1.1
import Qt.labs.shaders 1.0

Rectangle {
    signal measurementsSaved(string imagePath)
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

    ShaderEffectItem {
        property variant source: ShaderEffectSource { sourceItem: cam; hideSource: true }
        property real wiggleAmount: 0.0; //0.005
        anchors.fill: parent

        fragmentShader: "
        varying highp vec2 qt_TexCoord0;
        uniform sampler2D source;
        uniform highp float wiggleAmount;
        void main(void)
        {
            highp vec2 wiggledTexCoord = qt_TexCoord0;
            wiggledTexCoord.s += sin(4.0 * 3.141592653589 * wiggledTexCoord.t) * wiggleAmount;
            //gl_FragColor = texture2D(source, wiggledTexCoord.st);
            gl_FragColor = texture2D(source, wiggledTexCoord.st);
        }
        "
    }
    Camera {
        id: cam
        x: 0
        y: 0
        width: parent.width
        height: parent.height
        captureResolution: "900x506" // 3:2
        onImageSaved: measurementsSaved(path)

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
            cam.captureImage();
            //measurementsSaved();
        }
    }
}
