describe('crearPlaylist', () => {
  it('crearPlaylist', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.contains('Crear Playlist').click();
    cy.get('#Nombre').click();
    cy.get('#Nombre').type('Musica');
    cy.get('.btn:nth-child(2)').click();
    cy.get('div.alert-success').should('contain.text', '¡Playlist creada exitosamente!');
  });
});