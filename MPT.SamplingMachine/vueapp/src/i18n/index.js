import { createI18n } from 'vue-i18n'

const i18n = createI18n({
    locale: 'hi',   // default locale
    // translations
    messages: {
        hi: {
            titles: {
                terms: 'शर्तें',
                termsConfirmation: 'मैं सेवा की शर्तों और गोपनीयता नीति को स्वीकार करता हूं',
                phoneNumber: 'आपका फोन नंबर',
                checkPhoneWarning: 'कृपया अपना फोन नंबर दर्ज करें',
                checkPinWarning: 'ग़लत पिन या फ़ोन नंबर.<br>कृपया पुन: प्रयास करें',
                pinCode: 'पिन कोड',
                credit: 'श्रेय',
                lackOfCredit: 'अपर्याप्त क्रेडिट',
                outOfStock: 'स्टॉक ख़त्म',
                itemsCount: 'आइटम गिनती',
                takeYourProducts: 'अपने उत्पाद ले लो',
                goodBuy: 'अच्छी खरीद!',
                formatNumber: 'प्रारूप: xxxxxxxxxx',
                formatCode: 'प्रारूप: xxxx',
                idleModalTitle: 'क्या आप अभी भी यहीं हैं?',
                exit: 'बाहर निकलना',
                yes: 'हाँ',
                oos: 'क्षमा करें, कियोस्क सेवा से बाहर है'
            },
            buttons: {
                start: 'शुरू',
                next: 'अगला',
                add: 'जोड़ना',
                remove: 'निकालना',
                more: 'अधिक',
                dispense: 'उत्पाद जारी करें',
                sendSMS: 'एसएमएस भेजें',
                confirm: 'पुष्टि करना',
                sendPinCode: 'पिन कोड दोबारा भेजें'
            }
        },
        en: {
            titles: {
                terms: 'Terms',
                termsConfirmation: 'I accept the Terms of service and Privacy Policy',
                phoneNumber: 'Your phone number',
                checkPhoneWarning: 'Please enter your phone number',
                checkPinWarning: 'Wrong PIN or phone number.<br>Please try again',
                pinCode: 'PIN code',
                credit: 'Credit',
                lackOfCredit: 'insufficient<br/>credit',
                outOfStock: 'out of stock',
                itemsCount: 'Items count',
                takeYourProducts: 'Take your products',
                goodBuy: 'Good buy!',
                formatNumber: 'format: xxxxxxxxxx',
                formatCode: 'format: xxxx',
                idleModalTitle: 'Are you still here?',
                exit: 'Exit',
                yes: 'Yes',
                oos: 'Sorry, the kiosk is out of service'
            },
            buttons: {
                start: 'Start',
                next: 'Next',
                add: 'Add',
                remove: 'Remove',
                more: 'More',
                dispense: 'Issue products',
                sendSMS: 'Send SMS',
                confirm: 'Confirm',
                sendPinCode: 'Send PIN code again'
            }
        }
    }
})

export default i18n