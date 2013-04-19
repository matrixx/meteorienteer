#ifndef TAIVAANVAHTIFORM_H
#define TAIVAANVAHTIFORM_H

#include <QObject>
#include "taivaanvahtifield.h"
#include <QVector>

class TaivaanvahtiForm : public QObject
{
    Q_OBJECT
public:
    explicit TaivaanvahtiForm(QObject *parent = 0);
    void setFields(QVector<TaivaanvahtiField*> fields);
    QVector<TaivaanvahtiField*> fields();
signals:
    
public slots:

private:
    QVector<TaivaanvahtiField*> m_fields;
};

#endif // TAIVAANVAHTIFORM_H
