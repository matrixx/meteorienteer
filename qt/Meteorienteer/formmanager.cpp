#include "formmanager.h"
#include "taivaanvahtifield.h"
#include <QDebug>
#include <QStringList>
#include <QSettings>
#include <QDate>
#include <QTime>

FormManager::FormManager(QObject *parent) :
    QObject(parent), m_tv(NULL), m_currentField(-1), m_currentTaivaanvahtiField(NULL), m_currentId(""), m_currentValue("")
{
    m_tv = new Taivaanvahti;
    connect(m_tv, SIGNAL(formReceived(TaivaanvahtiForm*)), this, SLOT(receiveForm(TaivaanvahtiForm*)));
    m_filterList.append("observation_date");
    m_filterList.append("observation_start_hours");
    m_filterList.append("observation_coordinates");
    m_filterList.append("observation_location");
    m_filterList.append("user_name");
//    m_filterList.append("user_email");
//    m_filterList.append("user_phone");
//    m_filterList.append("observation_public");
//    m_filterList.append("observation_title");
//    m_filterList.append("observation_description");
//    m_filterList.append("observation_equipment");
    m_filterList.append("specific_havaintoajan_tarkkuus");
    m_filterList.append("specific_lennon_kesto");
    m_filterList.append("specific_sammumistapa");
    m_filterList.append("specific_ilmansuunta_katoamishetkellä");
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
        prefill();
        labelChanged();
        prefilledChanged();
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
        prefill();
        labelChanged();
        prefilledChanged();
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
// TODO: checkbox not yet in UI, use textareafield
        type = "TextAreaField.qml";
//        type = "CheckBoxField.qml";
        break;
    case TaivaanvahtiField::TYPE_SELECTION:
        type = "RadioButtonField.qml";
        break;
    case TaivaanvahtiField::TYPE_DATE:
// TODO: datepicker not yet in UI, use textareafield
        type = "TextAreaField.qml";
//        type = "DatePicker.qml";
        break;
    case TaivaanvahtiField::TYPE_COORDINATE:
// TODO: coordinate type not yet supported in UI, use textareafield
        type = "TextAreaField.qml";
//        type = "Coordinate.qml";
        break;
    case TaivaanvahtiField::TYPE_TIME:
// TODO: coordinate type not yet supported in UI, use textareafield
        type = "TextAreaField.qml";
//        type = "TimePicker.qml";
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
        return localizedLabel();
        //return m_currentTaivaanvahtiField->label();
    } else {
        return QString();
    }
}

QString FormManager::prefilled()
{
    return m_currentValue;
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
    return localizedValues();
//    return m_currentTaivaanvahtiField->values().values();
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

void FormManager::setCompass(qreal azimuth)
{
    Q_UNUSED(azimuth);
    // TODO: phone (N9 atleast) needs to be in specific position when compass is read
    // this needs specific UI instructions, meanwhile let user fill over the default
    m_compassPoint = "NorthEast";
}

void FormManager::setCoordinate(double lat, double lon)
{
    m_coordinate.clear();
    m_coordinate.append(QString::number(lat)).append(", ").append(QString::number(lon));
    qDebug() << "got new coordinate:" << m_coordinate;
}

void FormManager::receiveForm(TaivaanvahtiForm* form)
{
    qDebug() << "FormManager received form";
    m_form = form;
    form->setParent(this);
    filter();
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

void FormManager::prefill()
{
    if (m_currentId == "observation_date") {
        // default to current date (TODO: add datepicker to UI to allow to change it)
        m_currentValue = QDate::currentDate().toString("dd.MM.yyyy");
    } else if (m_currentId == "observation_location") {
        // TODO: save last inserted location, or possibly query for location
        // resolving using geocoordinate
        m_currentValue = "Tampere";
    } else if (m_currentId == "user_name") {
        // TODO: save the name when filled in first time and restore from settings
        m_currentValue = "Saija Eteläniemi";
    } else if (m_currentId == "observation_start_hours") {
        m_currentValue = QTime::currentTime().toString("hh:mm:ss ap");
    } else if (m_currentId == "observation_coordinates") {
        m_currentValue = m_coordinate;
    } else if (m_currentId == "specific_ilmansuunta_katoamishetkellä") {
        m_currentValue = m_compassPoint;
    } else {
        m_currentValue = "";
    }
}

QString FormManager::localizedLabel()
{
    qDebug() << "starting to localize label";
    // TODO: replace with proper localizing system
    QString localized = m_currentTaivaanvahtiField->label();
    if (m_currentId == "observation_date") {
        localized = "Observation date";
    } else if (m_currentId == "observation_start_hours") {
        localized = "Time";
    } else if (m_currentId == "observation_coordinates") {
        localized = "Coordinates";
    } else if (m_currentId == "observation_location") {
        localized = "City";
    } else if (m_currentId == "user_name") {
        localized = "Your name";
    } else if (m_currentId == "specific_havaintoajan_tarkkuus") {
        localized = "How precise is your time estimate?";
    } else if (m_currentId == "specific_lennon_kesto") {
        localized = "How long did the fireball's flight last?";
    } else if (m_currentId == "specific_sammumistapa") {
        localized = "How did your fireball fade away?";
    } else if (m_currentId == "specific_ilmansuunta_katoamishetkellä") {
        localized = "In which direction did the object disappear?";
    }
    qDebug() << "returning localized label:";
    qDebug() << "label:" << localized;
    return localized;
}

QStringList FormManager::localizedValues()
{
    QStringList localizedValues = m_currentTaivaanvahtiField->values().values();

    if (m_currentId == "specific_havaintoajan_tarkkuus") {
        localizedValues.clear();
        localizedValues.append("select");
        localizedValues.append("The	difference is one minute at maximum");
        localizedValues.append("The	difference can be about 2 minutes");
        localizedValues.append("The	difference can be about 5 minutes");
        localizedValues.append("The	difference can be about 15 minutes");
        localizedValues.append("The	difference can be from one to two hours");
        localizedValues.append("The difference can be from one to two days");
        localizedValues.append("The	month of the observation is uncertain");
        localizedValues.append("The	year of the observation is uncertain");
    } else if (m_currentId == "specific_lennon_kesto") {
        localizedValues.clear();
        localizedValues.append("select");
        localizedValues.append("less than one second");
        localizedValues.append("1 second");
        localizedValues.append("2 seconds");
        localizedValues.append("3 seconds");
        localizedValues.append("4 seconds");
        localizedValues.append("5 seconds");
        localizedValues.append("6 seconds");
        localizedValues.append("7 seconds");
        localizedValues.append("8 seconds");
        localizedValues.append("9 seconds");
        localizedValues.append("10 seconds");
        localizedValues.append("11-15 seconds");
        localizedValues.append("16-20 seconds");
        localizedValues.append("more than 20 seconds");
    } else if (m_currentId == "specific_sammumistapa") {
        localizedValues.clear();
        localizedValues.append("select");
        localizedValues.append("It disappeared behind an object (perhaps a tree or clouds)");
        localizedValues.append("Faded out quickly");
        localizedValues.append("Faded out slowly");
        localizedValues.append("Broke into pieces which then faded out");
    } else if (m_currentId == "specific_ilmansuunta_katoamishetkellä") {
        localizedValues.clear();
        localizedValues.append("select");
        localizedValues.append("North");
        localizedValues.append("NorthEast");
        localizedValues.append("East");
        localizedValues.append("SouthEast");
        localizedValues.append("South");
        localizedValues.append("SouthWest");
        localizedValues.append("West");
        localizedValues.append("NorthWest");
    }
    return localizedValues;
}
