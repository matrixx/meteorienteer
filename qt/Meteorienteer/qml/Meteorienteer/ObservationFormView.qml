// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1

Rectangle {
    id: formView;
    property variant formFields: 0;
    property int direction: 0;
    signal formSubmitSucceeded;
    signal formSubmitFailed;
    signal formCanceled;

    color: "black"
    anchors.fill: parent

/*
    Connections {
        target: mgr
        onFormReceived:
        {
            loadField();

            formFields = fields;
            for (var i = 0; i < formFields.length; i++) {
                console.log(formFields[i].id);
                console.log(formFields[i].label);
                console.log(formFields[i].info);
                console.log(formFields[i].infoUrl);
                console.log(formFields[i].isMandatory);
                console.log(formFields[i].type);
                for (var j = 0; j < formFields[i].valueListKeys.length; j++) {
                    console.log(formFields[i].valueListKeys[j]);
                    console.log(formFields[i].valueListValues[j]);
                }
            }
        }
    }
    */
    function loadField() {
        if (mgr.next()) {
            var fieldComponent;
            var fieldObject;
            fieldComponent = Qt.createComponent(mgr.type);
            fieldObject = fieldComponent.createObject(formView, { "anchors.fill" : formView });
            console.debug("loaded component");
            //
        } else {
            // show overview and
        }
    }
}
