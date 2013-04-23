#ifndef FORMMANAGER_H
#define FORMMANAGER_H

#include <QObject>
#include "taivaanvahtiform.h"
#include "taivaanvahti.h"

class FormManager : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString type READ type)
    Q_PROPERTY(QString id READ id)
    Q_PROPERTY(QString label READ label)

public:
    explicit FormManager(QObject *parent = 0);
    Q_INVOKABLE void getForm();
    Q_INVOKABLE int fieldCount();
    Q_INVOKABLE bool next();
    Q_INVOKABLE int currentField();
    Q_INVOKABLE QString type();
    Q_INVOKABLE QString id();
    Q_INVOKABLE QString label();
    Q_INVOKABLE void setValue(QString value);
    Q_INVOKABLE void setValueIndex(int index);
    Q_INVOKABLE QStringList values();
    Q_INVOKABLE void saveSetting(QString id, QString value);
    Q_INVOKABLE QString loadSetting(QString id);
    Q_INVOKABLE void submit();

signals:
    Q_INVOKABLE void formReceived();
    Q_INVOKABLE void submitted(bool success);

public slots:
    void receiveForm(TaivaanvahtiForm* form);
    void onSubmitted(bool success, int observationId, QString observationModificationKey);

private:
    void filter();

    Taivaanvahti* m_tv;
    TaivaanvahtiForm* m_form;
    QMap<TaivaanvahtiField*, QString> m_result;
    int m_currentField;
    TaivaanvahtiField* m_currentTaivaanvahtiField;
    QString m_currentId;
    QString m_currentValue;
    QStringList m_filterList;
};

#endif // FORMMANAGER_H
