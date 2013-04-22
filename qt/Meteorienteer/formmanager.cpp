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
}

void FormManager::getForm()
{
    m_result.clear();
    m_tv->getForm(1);
}

int FormManager::fieldCount()
{
    return m_form->fields().size();
}

bool FormManager::next()
{
    bool retval = false;
    if (m_currentField < m_form->fields().size() - 1) {
        m_currentField++;
        m_currentTaivaanvahtiField = m_form->fields()[m_currentField];
        m_currentId = m_currentTaivaanvahtiField->id();
        retval = true;
    }
    return retval;
}

int FormManager::currentField()
{
    return m_currentField;
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
    return "TextAreaField.qml";
//    return type;
}

QString FormManager::id()
{
    return m_currentId;
}

QString FormManager::label()
{
    return m_currentTaivaanvahtiField->label();
}

void FormManager::setValue(QString value)
{
    m_currentValue = value;
    m_result.insert(m_currentTaivaanvahtiField, value);
}

void FormManager::setValueIndex(int index)
{
    m_currentValue = index;
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
    if (!m_result.isEmpty()) {
        m_tv->submitForm(m_result, 1);
    }
}

void FormManager::receiveForm(TaivaanvahtiForm* form)
{
    qDebug() << "FormManager received form";
    m_form = form;
    form->setParent(this);
    //filter();
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
    emit formReceived();
}

void FormManager::onSubmitted(bool success, int observationId, QString observationModificationKey)
{
    qDebug() << "onSubmitted, success:" << success << "|" << observationId << "|" << observationModificationKey;
    delete m_form;
    m_form = NULL;
    m_result.clear();
    emit submitted(success);
}

void FormManager::filter()
{
    QVector<TaivaanvahtiField*> fields = m_form->fields();
    QVector<TaivaanvahtiField*> newFields;
    for (int i = 0; i < fields.size(); ++i) {
        // if we want only mandatory fields, filter out non-mandatory
        TaivaanvahtiField* field = fields.at(i);
        if (field->isMandatory()) {
            newFields.push_back(field);
        } else {
            delete field;
        }
    }
    m_form->setFields(newFields);
}
