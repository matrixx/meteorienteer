#include "taivaanvahtiform.h"

TaivaanvahtiForm::TaivaanvahtiForm(QObject *parent) :
    QObject(parent)
{
}

void TaivaanvahtiForm::setFields(QVector<TaivaanvahtiField *> fields)
{
    m_fields = fields;
}

QVector<TaivaanvahtiField *> TaivaanvahtiForm::fields()
{
    return m_fields;
}
