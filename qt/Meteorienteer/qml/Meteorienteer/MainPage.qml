import QtQuick 1.1
import com.nokia.meego 1.0

Page {
    id: page
    tools: commonTools

    property string title : "Meteorienteer"

    Image {
        id: pageHeader
        anchors {
            top: page.top
            left: page.left
            right: page.right
        }

        height: parent.width < parent.height ? 72 : 46
        width: parent.width
        source: "image://theme/meegotouch-view-header-fixed-inverted"
        z: 1

        Label {
            id: header
            anchors {
                verticalCenter: parent.verticalCenter
                left: parent.left
                leftMargin: 16
            }
            platformStyle: LabelStyle {
                fontFamily: "Nokia Pure Text Light"
                fontPixelSize: 32
            }
            text: page.title
        }
    }
    LocationDataProvider {
        id: measurements
    }

    Flickable {
        id: pageFlickableContent
        anchors {
            top: pageHeader.bottom
            bottom: page.bottom
            left: page.left
            right: page.right
            margins: 16
        }
        contentHeight: pageContent.height
        contentWidth: pageContent.width
        flickableDirection: Flickable.VerticalFlick
        Row {
            Column {
                id: pageContent
                width: page.width / 2 - pageFlickableContent.anchors.margins * 2
                spacing: 16

                Label {
                    wrapMode: Text.WordWrap
                    text: qsTr("Accelerometer reading: %1 | %2 | %3").arg(measurements.accelx).arg(measurements.accely).arg(measurements.accelz)
                }
                Label {
                    wrapMode: Text.WordWrap
                    text: qsTr("Compass azimuth in degrees: %1").arg(measurements.azimuth)
                }
                Label {
                    wrapMode: Text.WordWrap
                    text: qsTr("GPS coordinates: %1 | %2").arg(measurements.latitude).arg(measurements.longitude)
                }
            }
            Column {
                ObservationView {
                    id: viewFinder
                }
            }
        }
    }

    ScrollDecorator {
        flickableItem: pageFlickableContent
    }
}
