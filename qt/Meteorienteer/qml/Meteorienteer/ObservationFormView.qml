// import QtQuick 1.0 // to target S60 5th Edition or Maemo 5
import QtQuick 1.1
import tv 1.0

Rectangle {
    property variant formFields: 0;
    color: "black"
    Connections {
        target: taivaanvahti
        onFormReceived: {
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
}
