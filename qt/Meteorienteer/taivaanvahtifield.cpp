#include "taivaanvahtifield.h"
#include <QDebug>
#include <QDomNode>

TaivaanvahtiField::TaivaanvahtiField(QObject *parent) :
    QObject(parent), mandatory(false), fieldType(TYPE_NOT_SET)
{
}

void TaivaanvahtiField::parseFieldElement(QDomElement elem)
{
    fieldId = elem.firstChildElement("field_id").text();
    fieldLabel = elem.firstChildElement("field_label").text();
    mandatory = elem.firstChildElement("field_mandatory").text() == "1";
    fieldInfo = elem.firstChildElement("field_info").text();
    fieldInfoUrl = QUrl(elem.firstChildElement("field_info_url").text());

    QString fieldTypeString = elem.firstChildElement("field_type").text();

    qDebug() << "parsing field id:" << fieldId << "field type string:" << fieldTypeString;

    if(fieldTypeString=="text") fieldType = TYPE_TEXT;
    if(fieldTypeString.toLower()=="date") fieldType = TYPE_DATE;
    if(fieldTypeString.toLower()=="time") fieldType = TYPE_TIME;
    if(fieldTypeString=="coordinate") fieldType = TYPE_COORDINATE;
    if(fieldTypeString=="checkbox") fieldType = TYPE_CHECKBOX;
    if(fieldTypeString=="select") fieldType = TYPE_SELECTION;
    if(fieldTypeString=="specific_tulipallon_kirkkaus") fieldType = TYPE_SELECTION;
    if(fieldType == TYPE_NOT_SET) {
        qDebug() << Q_FUNC_INFO << "Warning: field type " << fieldTypeString << " not handled!";
    }
    qDebug() << "evaluated as:" << fieldType;
    QDomElement valuesElement = elem.firstChildElement("values");
    if(!valuesElement.isNull()) {
        QDomElement valueElement = valuesElement.firstChildElement("value");
        while(!valueElement.isNull()) {
            fieldValues.insert(valueElement.firstChildElement("value_id").text(), valueElement.firstChildElement("value_name").text());
            valueElement = valueElement.nextSiblingElement("value");
        }
    }
    if(!fieldValues.isEmpty()) fieldType = TYPE_SELECTION;
    // qDebug() << fieldId << fieldLabel << fieldValues;
}

void TaivaanvahtiField::createFieldElement(QDomElement elem, QString value, QDomDocument &doc)
{
    QString idString = id();
    // Workarounds:
    if(idString == "observation_start_hours") idString = "start_hours";
    if(idString == "observation_end_hours") idString = "end_hours";
    createFieldElement(elem, idString, value, doc);
}

void TaivaanvahtiField::createFieldElement(QDomElement elem, QString name, QString value, QDomDocument &doc)
{
    QDomElement fieldElement = doc.createElement("field");
    elem.appendChild(fieldElement);

    QDomElement fieldIdElement = doc.createElement("field_id");
    fieldElement.appendChild(fieldIdElement);

    QDomText fieldIdText = doc.createTextNode(name);
    fieldIdElement.appendChild(fieldIdText);

    QDomElement fieldValueElement = doc.createElement("field_value");
    fieldElement.appendChild(fieldValueElement);
    QDomText fieldValueText = doc.createTextNode(value);
    fieldValueElement.appendChild(fieldValueText);
}
