﻿// jQuery Mask Plugin v1.6.5
// github.com/igorescobar/jQuery-Mask-Plugin
(function (g) { "function" === typeof define && define.amd ? define(["jquery"], g) : g(window.jQuery || window.Zepto) })(function (g) {
    var z = function (b, f, d) {
        var l = this, x, y; b = g(b); f = "function" === typeof f ? f(b.val(), void 0, b, d) : f; l.init = function () {
            d = d || {}; l.byPassKeys = [9, 16, 17, 18, 36, 37, 38, 39, 40, 91]; l.translation = { 0: { pattern: /\d/ }, 9: { pattern: /\d/, optional: !0 }, "#": { pattern: /\d/, recursive: !0 }, A: { pattern: /[a-zA-Z0-9]/ }, S: { pattern: /[a-zA-Z]/ } }; l.translation = g.extend({}, l.translation, d.translation); l = g.extend(!0, {}, l, d); y =
                c.getRegexMask(); b.each(function () { !1 !== d.maxlength && b.attr("maxlength", f.length); d.placeholder && b.attr("placeholder", d.placeholder); b.attr("autocomplete", "off"); c.destroyEvents(); c.events(); var a = c.getCaret(); c.val(c.getMasked()); c.setCaret(a + c.getMaskCharactersBeforeCount(a, !0)) })
        }; var c = {
            getCaret: function () {
                var a; a = 0; var e = b.get(0), c = document.selection, e = e.selectionStart; if (c && !~navigator.appVersion.indexOf("MSIE 10")) a = c.createRange(), a.moveStart("character", b.is("input") ? -b.val().length : -b.text().length),
                    a = a.text.length; else if (e || "0" === e) a = e; return a
            }, setCaret: function (a) { if (b.is(":focus")) { var e; e = b.get(0); e.setSelectionRange ? e.setSelectionRange(a, a) : e.createTextRange && (e = e.createTextRange(), e.collapse(!0), e.moveEnd("character", a), e.moveStart("character", a), e.select()) } }, events: function () {
                b.on("keydown.mask", function () { x = c.val() }); b.on("keyup.mask", c.behaviour); b.on("paste.mask drop.mask", function () { setTimeout(function () { b.keydown().keyup() }, 100) }); b.on("change.mask", function () {
                    b.data("changeCalled",
                        !0)
                }); b.on("blur.mask", function (a) { a = g(a.target); a.prop("defaultValue") !== a.val() && (a.prop("defaultValue", a.val()), a.data("changeCalled") || a.trigger("change")); a.data("changeCalled", !1) }); b.on("focusout.mask", function () { d.clearIfNotMatch && !y.test(c.val()) && c.val("") })
            }, getRegexMask: function () {
                var a = [], e, b, c, d, k; for (k in f) (e = l.translation[f[k]]) ? (b = e.pattern.toString().replace(/.{1}$|^.{1}/g, ""), c = e.optional, (e = e.recursive) ? (a.push(f[k]), d = { digit: f[k], pattern: b }) : a.push(c || e ? b + "?" : b)) : a.push("\\" +
                    f[k]); a = a.join(""); d && (a = a.replace(RegExp("(" + d.digit + "(.*" + d.digit + ")?)"), "($1)?").replace(RegExp(d.digit, "g"), d.pattern)); return RegExp(a)
            }, destroyEvents: function () { b.off("keydown.mask keyup.mask paste.mask drop.mask change.mask blur.mask focusout.mask").removeData("changeCalled") }, val: function (a) { var e = b.is("input"); return 0 < arguments.length ? e ? b.val(a) : b.text(a) : e ? b.val() : b.text() }, getMaskCharactersBeforeCount: function (a, e) {
                for (var b = 0, c = 0, d = f.length; c < d && c < a; c++) l.translation[f.charAt(c)] || (a =
                    e ? a + 1 : a, b++); return b
            }, determineCaretPos: function (a, b, d, h) { return l.translation[f.charAt(Math.min(a - 1, f.length - 1))] ? Math.min(a + d - b - h, d) : c.determineCaretPos(a + 1, b, d, h) }, behaviour: function (a) {
                a = a || window.event; var b = a.keyCode || a.which; if (-1 === g.inArray(b, l.byPassKeys)) {
                    var d = c.getCaret(), f = c.val(), n = f.length, k = d < n, p = c.getMasked(), m = p.length, q = c.getMaskCharactersBeforeCount(m - 1) - c.getMaskCharactersBeforeCount(n - 1); p !== f && c.val(p); !k || 65 === b && a.ctrlKey || (8 !== b && 46 !== b && (d = c.determineCaretPos(d, n, m, q)),
                        c.setCaret(d)); return c.callbacks(a)
                }
            }, getMasked: function (a) {
                var b = [], g = c.val(), h = 0, n = f.length, k = 0, p = g.length, m = 1, q = "push", s = -1, r, u; d.reverse ? (q = "unshift", m = -1, r = 0, h = n - 1, k = p - 1, u = function () { return -1 < h && -1 < k }) : (r = n - 1, u = function () { return h < n && k < p }); for (; u();) { var v = f.charAt(h), w = g.charAt(k), t = l.translation[v]; if (t) w.match(t.pattern) ? (b[q](w), t.recursive && (-1 === s ? s = h : h === r && (h = s - m), r === s && (h -= m)), h += m) : t.optional && (h += m, k -= m), k += m; else { if (!a) b[q](v); w === v && (k += m); h += m } } a = f.charAt(r); n !== p + 1 || l.translation[a] ||
                    b.push(a); return b.join("")
            }, callbacks: function (a) { var e = c.val(), g = c.val() !== x; if (!0 === g && "function" === typeof d.onChange) d.onChange(e, a, b, d); if (!0 === g && "function" === typeof d.onKeyPress) d.onKeyPress(e, a, b, d); if ("function" === typeof d.onComplete && e.length === f.length) d.onComplete(e, a, b, d) }
        }; l.remove = function () { var a = c.getCaret(), b = c.getMaskCharactersBeforeCount(a); c.destroyEvents(); c.val(l.getCleanVal()).removeAttr("maxlength"); c.setCaret(a - b) }; l.getCleanVal = function () { return c.getMasked(!0) }; l.init()
    };
    g.fn.mask = function (b, f) { this.unmask(); return this.each(function () { g(this).data("mask", new z(this, b, f)) }) }; g.fn.unmask = function () { return this.each(function () { try { g(this).data("mask").remove() } catch (b) { } }) }; g.fn.cleanVal = function () { return g(this).data("mask").getCleanVal() }; g("*[data-mask]").each(function () {
        var b = g(this), f = {}; "true" === b.attr("data-mask-reverse") && (f.reverse = !0); "false" === b.attr("data-mask-maxlength") && (f.maxlength = !1); "true" === b.attr("data-mask-clearifnotmatch") && (f.clearIfNotMatch =
            !0); b.mask(b.attr("data-mask"), f)
    })
});