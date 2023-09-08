export default {
    props: {
        currentLanguage: {
            type: Object,
            required: false
        },
        // reflects max samples count per session
        credit: {
            type: Number,
            required: true
        },
        idleTimeoutSec: {
            type: Number,
            required: true
        },
        canLogOff: {
            type: Boolean,
            required: true
        }
    }
}