// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import QtMobility.sensors 1.2
import QtMobility.location 1.2

Item {
    property real accelx: 0
    property real accely: 0
    property real accelz: 0
    property real azimuth: 0
    property double latitude: 0.0
    property double longitude: 0.0

    Accelerometer {
        onReadingChanged: {
            accelx = reading.x
            accely = reading.y
            accelz = reading.z
        }
        active: true
    }
    Compass {
        onReadingChanged: {
            azimuth = reading.azimuth
        }
        active: true
    }
    PositionSource {
        id: positionSource
        updateInterval: 1000
        active: true
        onPositionChanged: {
            latitude = positionSource.position.coordinate.latitude
            longitude = positionSource.position.coordinate.longitude
        }
    }
}
