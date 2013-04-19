#include "taivaanvahtitest.h"
#include <QDebug>
#include "taivaanvahtifield.h"
#include "taivaanvahtiform.h"

TaivaanvahtiTest::TaivaanvahtiTest(QObject *parent) :
    QObject(parent)
{
    connect(&tv, SIGNAL(formReceived(TaivaanvahtiForm*)), this, SLOT(formReceived(TaivaanvahtiForm*)));
}

void TaivaanvahtiTest::runTest()
{
    qDebug() << Q_FUNC_INFO << "Getting form..";
    tv.getForm(1);
}

void TaivaanvahtiTest::formReceived(TaivaanvahtiForm* form)
{
    QVector<TaivaanvahtiField*> fields = form->fields();
    qDebug() << Q_FUNC_INFO << "Form received with " << fields.size() << " fields";
    Taivaanvahti::FormData formData;
    foreach(TaivaanvahtiField *field, fields) {
        qDebug() << field->id() << field->values();

        QString value = "testi testitesti Testi";
        switch(field->type()) {
        case TaivaanvahtiField::TYPE_CHECKBOX:
            value = "true";
            break;
        case TaivaanvahtiField::TYPE_COORDINATE:
            value = "lat=64.23414, lon=20.13445";
            break;
        case TaivaanvahtiField::TYPE_DATE:
            value = "2013-01-23";
            break;
        case TaivaanvahtiField::TYPE_SELECTION: {
            TaivaanvahtiField::ValueList vl = field->values();
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
        if(field->id()=="observation_end_hours") value= "13:30";
        if(field->id()=="specific_lentokulma") value="100";
        bool add = true;
        if(field->id().contains("minutes")) add=false;
        if(field->id()=="specific_tulipallon_kirkkaus") add = false;
        if(field->id()=="observation_demo_picture") add=false;
        if(field->id()=="observation_public") add=false;
        if(field->id()=="observation_user_pictures") add=false;
        if(field->id()=="observation_equipment") add=false;
        if(field->id()=="specific_havaintoajan_tarkkuus") add=false;
        if(add) {
            qDebug() << field->id() << field->type() << value;
            formData.insert(field, value);
        }
    }
    qDebug() << Q_FUNC_INFO << "Submitting " << formData.size() << " values..";
    tv.submitForm(formData, 1);
    qDebug() << Q_FUNC_INFO << "Submitting finished";
}
