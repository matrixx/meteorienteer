// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import QtMultimediaKit 1.1
import Qt.labs.shaders 1.0

Rectangle {
    id: obview
    signal measurementsSaved(string imagePath)
    anchors.fill: parent
    color: "black"
    property bool toolBarEnabled: false;
    property bool shooterEnabled: true;
    property int toolBarHeight:100;
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
        id: hlitem;

        //property variant source: ShaderEffectSource { sourceItem: cam; hideSource: true }
        property real wiggleAmount: 0.005;
        property real highLightThreshold: 0.7;
        property bool enabled: obview.toolBarEnabled
        onHighLightThresholdChanged: console.debug("onHighLightThresholdChanged:"+highLightThreshold)
        anchors.fill: parent

        fragmentShader: "
        varying highp vec2 qt_TexCoord0;
        uniform highp float highLightThreshold;
        uniform sampler2D source;
        uniform highp float wiggleAmount;
        uniform bool enabled;
        void main(void)
        {
            if (enabled)
            {
                highp vec2 wiggledTexCoord = qt_TexCoord0.st;
                wiggledTexCoord.s += sin(4.0 * 3.141592653589 * wiggledTexCoord.t) * wiggleAmount;
                //gl_FragColor = texture2D(source, wiggledTexCoord.st);
                gl_FragColor = texture2D(source, qt_TexCoord0.st);
                gl_FragColor.a = 1.0;
                gl_FragColor.rgb = step(highLightThreshold, gl_FragColor.rgb) + gl_FragColor.rgb;
            } else
                gl_FragColor = texture2D(source, qt_TexCoord0.st);
        }
        "
    }
    Image {
        state: "normal"
        visible: obview.shooterEnabled
        anchors.right: obview.right
        anchors.verticalCenter: parent.verticalCenter
        anchors.rightMargin: 20
        width: 100;
        height: 100;
        z:30
        id: shootButton
        MouseArea {
            anchors.fill: parent
         onPressed: {
             shootButton.state = "active"
             cam.captureImage();
         }
         onReleased: {
             shootButton.state = "normal"
         }
        }
        states: [
            State {
                name: "normal"
                PropertyChanges {
                    target: shootButton
                    source: "qrc:/gfx/buttonNormal.png"
                }
            },
            State {
                name: "hover"
                PropertyChanges {
                    target: shootButton
                    source: "qrc:/gfx/buttonHover.png"
                }
            },
            State {
                name: "active"
                PropertyChanges {
                    target: shootButton
                    source: "qrc:/gfx/buttonActive.png"
                }
            }
        ]
    }

    Rectangle {
        visible: obview.toolBarEnabled
        anchors.top: cam.bottom;
        anchors.left: cam.left
        anchors.right: cam.right
        height: cam.toolBarHeight;
        z:30;
        id: sliderIt
        color: "black"
        Rectangle {
            id: grab
            color: "#4574b5"
            height: cam.toolBarHeight
            width: 100;
            radius: 20
            border.color: "darkgrey"
            Component.onCompleted: grab.x = sliderIt.width * hlitem.highLightThreshold
//            x: sliderIt.width * hlitem.highLightThreshold;
        MouseArea {
            id: moveArea
            anchors.fill: parent;
            drag.target: parent;
            drag.axis: "XAxis"
            drag.maximumX: sliderIt.width - grab.width
            drag.minimumX: 0
            onMouseXChanged: {
                hlitem.highLightThreshold = (moveArea.mapToItem(sliderIt,mouseX, mouseY).x/(sliderIt.width-grab.width));
                //console.debug("MouseX:"+ mouseX + " sliderwidth:" + sliderIt.width)
                }
            }
        }
    }
    Camera {
        id: cam
        x: 0
        y: 0
        width: parent.width
        height: obview.toolBarEnabled ? obview.height - obview.toolBarHeight : obview.height ;
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
