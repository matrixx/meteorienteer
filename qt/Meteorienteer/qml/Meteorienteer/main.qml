import QtQuick 1.1
import com.nokia.meego 1.0

Window {
    id: appWindow

    StartView {
        id: startView
        visible: true
        onCreateObservation: {
            observationView.visible = true
            startView.visible = false
        }
    }
    ObservationView {
        id: observationView
        visible: false
    }

    QueryDialog {
        id: aboutDialog
        titleText: "Meteorienteer"
        message: "(C) [2013] [Meteorienteers]\n[0.1]"
    }
}
