#ifndef FORMMANAGER_H
#define FORMMANAGER_H

#include <QObject>
#include "taivaanvahtiform.h"
#include "taivaanvahti.h"

class FormManager : public QObject
{
    Q_OBJECT
    Q_PROPERTY(QString type READ type)
    Q_PROPERTY(QString label READ label NOTIFY labelChanged)

public:
    // Form submission data. Field, value
    typedef QPair<TaivaanvahtiField*, QString> FieldValue;

    explicit FormManager(QObject *parent = 0);
    Q_INVOKABLE void getForm();
    Q_INVOKABLE int fieldCount();
    Q_INVOKABLE bool previous();
    Q_INVOKABLE bool next();
    Q_INVOKABLE QString type();
    Q_INVOKABLE QString label();
    Q_INVOKABLE QString setValue(QString value);
    Q_INVOKABLE QString setValueIndex(int index);
    Q_INVOKABLE QStringList values();
    Q_INVOKABLE void submit();
    Q_INVOKABLE void reset();

signals:
    void formReceived();
    void submitted(bool success);
    void labelChanged();

public slots:
    void receiveForm(TaivaanvahtiForm* form);
    void onSubmitted(bool success, int observationId, QString observationModificationKey);

private:
    void filter();
    QString validate(QString id, QString value);
    void saveSetting(QString id, QString value);
    QString loadSetting(QString id);

    Taivaanvahti* m_tv;
    TaivaanvahtiForm* m_form;
    // field, value, value index
    QList<QPair<FieldValue, int> > m_result;
    int m_currentField;
    TaivaanvahtiField* m_currentTaivaanvahtiField;
    QString m_currentId;
    QString m_currentValue;
    QStringList m_filterList;
};

#endif // FORMMANAGER_H
