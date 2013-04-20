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
        onMeasurementsSaved: {
            arrowPlotterView.imageUrl = imagePath;
            arrowPlotterView.visible = true
            observationView.visible = false
        }
    }
    ArrowPlotter {
        id: arrowPlotterView
        visible: false
        onDirectionSelected: {
            observationFormView.visible = true
            arrowPlotterView.visible = false
            observationFormView.direction = direction;
            taivaanvahti.getForm(1);
        }
    }
    ObservationFormView {
        id: observationFormView
        visible: false
    }

    QueryDialog {
        id: aboutDialog
        titleText: "Meteorienteer"
        message: "(C) [2013] [Meteorienteers]\n[0.1]"
    }
}
