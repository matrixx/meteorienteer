#include "formmanager.h"
#include "taivaanvahtifield.h"
#include <QDebug>
#include <QStringList>
#include <QSettings>

FormManager::FormManager(QObject *parent) :
    QObject(parent), m_tv(NULL), m_currentField(-1), m_currentTaivaanvahtiField(NULL), m_currentId(""), m_currentValue("")
{
    m_tv = new Taivaanvahti;
    connect(m_tv, SIGNAL(formReceived(TaivaanvahtiForm*)), this, SLOT(receiveForm(TaivaanvahtiForm*)));
//    m_filterList.append("observation_date");
//    m_filterList.append("observation_start_hours");
//    m_filterList.append("observation_coordinates");
//    m_filterList.append("observation_location");
//    m_filterList.append("user_name");
//    m_filterList.append("user_email");
//    m_filterList.append("user_phone");
//    m_filterList.append("observation_public");
    m_filterList.append("observation_title");
    m_filterList.append("observation_description");
//    m_filterList.append("observation_equipment");
    m_filterList.append("specific_havaintoajan_tarkkuus");
    m_filterList.append("specific_lennon_kesto");
    m_filterList.append("specific_sammumistapa");
//    m_filterList.append("specific_ilmansuunta_katoamishetkellä");
//    m_filterList.append("specific_korkeus_katoamishetkellä");
//    m_filterList.append("specific_lentokulma");
}

void FormManager::getForm()
{
    m_result.clear();
    m_tv->getForm(1);
}

int FormManager::fieldCount()
{
    if (m_form != NULL) {
        return m_form->fields().size();
    } else {
        return 0;
    }
}

bool FormManager::previous()
{
    bool retval = false;
    if (m_currentField > 0) {
        m_currentField--;
        m_currentTaivaanvahtiField = m_form->fields().at(m_currentField);
        m_currentId = m_currentTaivaanvahtiField->id();
        labelChanged();
        retval = true;
    }
    return retval;
}

bool FormManager::next()
{
    bool retval = false;
    if (m_currentField < m_form->fields().size() - 1) {
        m_currentField++;
        m_currentTaivaanvahtiField = m_form->fields().at(m_currentField);
        m_currentId = m_currentTaivaanvahtiField->id();
        labelChanged();
        retval = true;
    }
    return retval;
}

QString FormManager::type()
{
    QString type = "TextAreaField.qml";
    switch(m_currentTaivaanvahtiField->type()) {
    case TaivaanvahtiField::TYPE_TEXT:
        type = "TextAreaField.qml";
        break;
    case TaivaanvahtiField::TYPE_CHECKBOX:
        type = "CheckBoxField.qml";
        break;
    case TaivaanvahtiField::TYPE_SELECTION:
        type = "RadioButtonField.qml";
        break;
    case TaivaanvahtiField::TYPE_DATE:
        type = "DatePicker.qml";
        break;
    case TaivaanvahtiField::TYPE_COORDINATE:
        type = "Coordinate.qml";
        break;
    case TaivaanvahtiField::TYPE_TIME:
        type = "TimePicker.qml";
        break;
    default:
        type = "TextAreaField.qml";
        break;
    };
//    return "TextAreaField.qml";
    return type;
}

QString FormManager::label()
{
    if (m_currentTaivaanvahtiField != NULL) {
        return m_currentTaivaanvahtiField->label();
    } else {
        return QString();
    }
}

QString FormManager::setValue(QString value)
{
    QString retval = validate(m_currentId, value);
    if (retval.isEmpty()) {
        m_currentValue = value;
        m_result[m_currentField].first.second = value;
    }
    return retval;
}

QString FormManager::setValueIndex(int index)
{
    QString retval = "";
    if (index >= values().size() || index < 0) {
        retval = "index out of range";
    } else {
        retval = validate(m_currentId, values().at(index));
    }
    if (retval.isEmpty()) {
        m_currentValue = m_currentTaivaanvahtiField->values().keys().at(index);
        m_result[m_currentField].first.second = m_currentValue;
        m_result[m_currentField].second = index;
        qDebug() << "setting value index value:" << m_currentValue;
    }
    return retval;
}

QStringList FormManager::values()
{
    return m_currentTaivaanvahtiField->values().values();
}

void FormManager::saveSetting(QString id, QString value)
{
    if (!id.isEmpty() && !value.isEmpty()) {
        QSettings settings;
        settings.setValue(id, value);
    }
}

QString FormManager::loadSetting(QString id)
{
    QSettings settings;
    if (!id.isEmpty() && settings.contains(id)) {
        return settings.value(id).toString();
    } else {
        return QString();
    }
}

void FormManager::submit()
{
    qDebug() << "submitting:";
    QMap<TaivaanvahtiField*, QString> values;
    for (int i = 0; i < m_result.size(); ++i) {
        QPair<TaivaanvahtiField*, QString> field = m_result.at(i).first;
        values.insert(field.first, field.second);
        qDebug() << field.first->id() << field.second;
    }
    // TODO: this fakes submission until properly tested
    emit submitted(true);
//    m_tv->submitForm(values, 1);
}

void FormManager::reset()
{
    delete m_form;
    m_form = NULL;
    m_currentTaivaanvahtiField = NULL;
    m_result.clear();
}

void FormManager::receiveForm(TaivaanvahtiForm* form)
{
    qDebug() << "FormManager received form";
    m_form = form;
    form->setParent(this);
    filter();
    /*
    for (int i = 0; i < m_form->fields().size(); ++i) {
        qDebug() << "field number:" << i;
        TaivaanvahtiField* field = m_form->fields()[i];

        qDebug() << field->id() << field->label() << field->info() << field->infoUrl() << field->type() << field->isMandatory();
        qDebug() << "values";
        QStringList keys = field->values().keys();
        QStringList values = field->values().values();

        for (int j = 0; j < keys.size(); ++j) {
            qDebug() << keys[j] << values[j];
        }
    }
    */
    emit formReceived();
}

void FormManager::onSubmitted(bool success, int observationId, QString observationModificationKey)
{
    qDebug() << "onSubmitted, success:" << success << "|" << observationId << "|" << observationModificationKey;
    // TODO: handle submit fail, do not reset if failed
    reset();
    emit submitted(success);
}

void FormManager::filter()
{
    QVector<TaivaanvahtiField*> fields = m_form->fields();
    QVector<TaivaanvahtiField*> newFields;
    for (int i = 0; i < fields.size(); ++i) {
        TaivaanvahtiField* field = fields.at(i);
        if (m_filterList.contains(field->id())) {
            newFields.push_back(field);
            m_result.push_back(qMakePair(qMakePair(field, QString()), 0));
        } else {
            delete field;
        }
    }
    m_form->setFields(newFields);
}

QString FormManager::validate(QString /*id*/, QString /*value*/)
{
    QString retval = "";
    return retval;
}
