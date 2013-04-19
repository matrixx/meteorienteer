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
            observationFormView.visible = true
            observationView.visible = false
            taivaanvahti.getForm(1);
        }
    }
    ObservationFormView {
        id: observationFormView
        visible: false
        Connections {
            target: taivaanvahti
            onFormReceived: {
                observationFormView.formFields = fields;
                for (var i = 0; i < observationFormView.formFields.length; i++)
                    console.log(observationFormView.formFields[i].label);
            }
        }
    }

    QueryDialog {
        id: aboutDialog
        titleText: "Meteorienteer"
        message: "(C) [2013] [Meteorienteers]\n[0.1]"
    }
}
