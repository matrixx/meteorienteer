#ifndef FORMMANAGER_H
#define FORMMANAGER_H

#include <QObject>
#include "taivaanvahtiform.h"

class FormManager : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString type READ type)
    Q_PROPERTY(QString id READ id)
    Q_PROPERTY(QString label READ label)

public:
    explicit FormManager(QObject *parent = 0);
    Q_INVOKABLE int fieldCount();
    Q_INVOKABLE void next();
    Q_INVOKABLE int currentField();
    Q_INVOKABLE QString type();
    Q_INVOKABLE QString id();
    Q_INVOKABLE QString label();
    Q_INVOKABLE void setValue(QString value);
    Q_INVOKABLE void setValueIndex(int index);
    Q_INVOKABLE QStringList values();
signals:
    void formReveiced();
public slots:
    void receiveForm(TaivaanvahtiForm* form);
private:
    void filter();

    TaivaanvahtiForm* m_form;
    int m_currentField;
    TaivaanvahtiField* m_currentTaivaanvahtiField;
    QString m_currentId;
    QString m_currentValue;
};

#endif // FORMMANAGER_H
