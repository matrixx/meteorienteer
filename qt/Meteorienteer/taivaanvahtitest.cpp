#include "taivaanvahtitest.h"
#include <QDebug>
#include "taivaanvahtifield.h"

TaivaanvahtiTest::TaivaanvahtiTest(QObject *parent) :
    QObject(parent)
{
    connect(&tv, SIGNAL(formReceived(QVector<TaivaanvahtiField*> &)), this, SLOT(formReceived(QVector<TaivaanvahtiField*> &)));
}

void TaivaanvahtiTest::runTest()
{
    qDebug() << Q_FUNC_INFO << "Getting form..";
    tv.getForm(1);
}

void TaivaanvahtiTest::formReceived(QVector<TaivaanvahtiField*> &fields)
{
    qDebug() << Q_FUNC_INFO << "Form received with " << fields.size() << " fields";
    Taivaanvahti::FormData formData;
    foreach(TaivaanvahtiField *field, fields) {
        QString value = "testitestitesti";
        switch(field->type()) {
        case TaivaanvahtiField::TYPE_CHECKBOX:
            value = "true";
            break;
        case TaivaanvahtiField::TYPE_COORDINATE:
            value = "lat=64.23414, lon=20.13445";
            break;
        case TaivaanvahtiField::TYPE_DATE:
            value = "2013-11-23";
            break;
        case TaivaanvahtiField::TYPE_SELECTION: {
            ValueList vl = field->values();
            value = field->values().keys().last();
            break;
        }
        case TaivaanvahtiField::TYPE_TIME:
            value = "12:34";
            break;
        case TaivaanvahtiField::TYPE_TEXT:
            break;
        case TaivaanvahtiField::TYPE_NOT_SET:
            qDebug() << Q_FUNC_INFO << "Warning: Type not set for a field!";
        }
        if(field->id()=="user_email") value = "testi@spaceship.fi";
        if(field->id()=="observation_location") value = "tampere";
        if(field->id()=="user_phone") value = "0407572533";
        if(field->id()=="user_team") value = "Tampereen Ursa";
        if(field->id()=="observation_showiness") value = "1";
        qDebug() << field->id() << field->type() << value;
        formData.insert(field, value);
    }
    qDebug() << Q_FUNC_INFO << "Submitting " << formData.size() << " values..";
    tv.submitForm(formData, 1);
    qDebug() << Q_FUNC_INFO << "Submitting finished";
}
