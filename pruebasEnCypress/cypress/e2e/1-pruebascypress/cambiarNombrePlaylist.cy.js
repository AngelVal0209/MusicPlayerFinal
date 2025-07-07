describe('cambiarNombrePlaylist', () => {
  it('cambiarNombrePlaylist', () => {
    cy.visit('https://localhost:7083/');
    cy.get('#CorreoElectronico').click();
    cy.get('#CorreoElectronico').type('test@test.com');
    cy.get('#Contrasena').click();
    cy.get('#Contrasena').type('test123');
    cy.get('.btn-login').click();
    cy.contains('Musica').click();
    cy.contains('Volver a Mis Playlists').click();
    cy.contains('Editar').click();
    cy.get('#Nombre').click();
    cy.get('#Nombre').type('Musica Editada');
    cy.get('.btn:nth-child(3)').click();
    cy.get('div.alert-success').should('contain.text', '¡Playlist actualizada exitosamente!');
  });
});