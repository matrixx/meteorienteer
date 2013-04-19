#ifndef FORMMANAGER_H
#define FORMMANAGER_H

#include <QObject>
#include "taivaanvahtiform.h"

class FormManager : public QObject
{
    Q_OBJECT
public:
    explicit FormManager(QObject *parent = 0);
    
signals:
    
public slots:
    void receiveForm(TaivaanvahtiForm* form);
private:
    TaivaanvahtiForm* m_form;
};

#endif // FORMMANAGER_H
